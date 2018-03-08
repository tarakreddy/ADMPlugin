using System;
using System.Collections.Generic;
using System.Linq;
using AgGateway.ADAPT.ApplicationDataModel.Equipment;
using AgGateway.ADAPT.Representation.RepresentationSystem;
using NUnit.Framework;

namespace AgGateway.ADAPT.AcceptanceTest
{
    public class AssertDevices
    {
        public static void VerifyDeviceElement(List<DeviceElement> catalogDeviceElements, string expectedDescription, 
            string expectedSerialNumber, string expectedGuid, string expectedDeviceClassification, DeviceElementTypeEnum expectedType)
        {
            var deviceElement = catalogDeviceElements.Find(x => x.Id.UniqueIds.Exists(id => id.Id == expectedGuid));
            Assert.IsNotNull(deviceElement);

            Assert.AreEqual(expectedDescription, deviceElement.Description);
            Assert.AreEqual(expectedSerialNumber, deviceElement.SerialNumber);
            Assert.AreEqual(expectedDeviceClassification, deviceElement.DeviceClassification.Value.Value);
            Assert.AreEqual(expectedType, deviceElement.DeviceElementType);
        }

        public static void VerifyDeviceModel(List<DeviceModel> catalogDeviceModels, string expectedDescription, string expectedGuid)
        {
            var model = catalogDeviceModels.Find(x => x.Id.UniqueIds.Exists(id => id.Id == expectedGuid));
            Assert.IsNotNull(model);

            Assert.AreEqual(expectedDescription, model.Description);
        }

        public static void VerifyMachineConfiguration(List<Connector> catalogConnectors, List<DeviceElement> catalogDeviceElements, 
            List<DeviceElementConfiguration> catalogDeviceElementConfigurations, List<HitchPoint> catalogHitchPoints, string deviceElementDescription, 
            HitchTypeEnum expectedHitchTypeEnum, OriginAxleLocationEnum expectedOriginAxleLocationEnum, Dictionary<string, Tuple<double, string>> expectedOffsets)
        {
            var configuration = VerifyDeviceElementConfiguration(catalogConnectors, catalogDeviceElements, catalogHitchPoints,
                catalogDeviceElementConfigurations, deviceElementDescription, expectedHitchTypeEnum);

            Assert.IsInstanceOf<MachineConfiguration>(configuration);

            var machineConfiguration = (MachineConfiguration) configuration;

            Assert.AreEqual(expectedOriginAxleLocationEnum, machineConfiguration.OriginAxleLocation);

            var gpsReceiverXOffsetDomainId = RepresentationInstanceList.vrGPSToNonSteeringAxleOffset.DomainId;
            if (expectedOffsets.ContainsKey(gpsReceiverXOffsetDomainId))
            {
                var expectedOffset = expectedOffsets[gpsReceiverXOffsetDomainId];
                var actualOffset = machineConfiguration.GpsReceiverXOffset;

                AssertValue.VerifyNumericRepresentationValue(expectedOffset.Item1, expectedOffset.Item2, gpsReceiverXOffsetDomainId, actualOffset);
            }

            var gpsReceiverYOffsetDomainId = RepresentationInstanceList.vrReceiverOffset.DomainId;
            if (expectedOffsets.ContainsKey(gpsReceiverYOffsetDomainId))
            {
                var expectedOffset = expectedOffsets[gpsReceiverYOffsetDomainId];
                var actualOffset = machineConfiguration.GpsReceiverYOffset;

                AssertValue.VerifyNumericRepresentationValue(expectedOffset.Item1, expectedOffset.Item2, gpsReceiverYOffsetDomainId, actualOffset);
            }
        }

        public static void VerifyImplementConfiguration(List<Connector> catalogConnectors, List<DeviceElement> catalogDeviceElements, 
            List<DeviceElementConfiguration> catalogDeviceElementConfigurations, List<HitchPoint> catalogHitchPoints, string deviceElementDescription, 
            Dictionary<string, Tuple<double, string>> expectedOffsets)
        {
            var configuration = VerifyDeviceElementConfiguration(catalogConnectors, catalogDeviceElements, catalogHitchPoints,
                catalogDeviceElementConfigurations, deviceElementDescription, null);
            
            Assert.IsInstanceOf<ImplementConfiguration>(configuration);

            var implementConfiguration = (ImplementConfiguration)configuration;

            var equipmentWidthDomainId = RepresentationInstanceList.vrEquipmentWidth.DomainId;
            if (expectedOffsets.ContainsKey(equipmentWidthDomainId))
            {
                var expectedWidth = expectedOffsets[equipmentWidthDomainId];
                var actualWidth = implementConfiguration.Width;

                AssertValue.VerifyNumericRepresentationValue(expectedWidth.Item1, expectedWidth.Item2, equipmentWidthDomainId, actualWidth);
            }

            var trackSpacingDomainId = RepresentationInstanceList.vrTrackSpacing.DomainId;
            if (expectedOffsets.ContainsKey(trackSpacingDomainId))
            {
                var expectedTrackSpacing = expectedOffsets[trackSpacingDomainId];
                var actualTrackSpacing = implementConfiguration.TrackSpacing;

                AssertValue.VerifyNumericRepresentationValue(expectedTrackSpacing.Item1, expectedTrackSpacing.Item2, trackSpacingDomainId, actualTrackSpacing);
            }

            var physicalImplementWidthDomainId = RepresentationInstanceList.vrPhysicalImplementWidth.DomainId;
            if (expectedOffsets.ContainsKey(physicalImplementWidthDomainId))
            {
                var expectedPhysialWidth = expectedOffsets[physicalImplementWidthDomainId];
                var actualPhysicalWidth = implementConfiguration.PhysicalWidth;

                AssertValue.VerifyNumericRepresentationValue(expectedPhysialWidth.Item1, expectedPhysialWidth.Item2, physicalImplementWidthDomainId, actualPhysicalWidth);
            }

            var implementLengthDomainId = RepresentationInstanceList.vrImplementLength.DomainId;
            if (expectedOffsets.ContainsKey(implementLengthDomainId))
            {
                var expectedImplementLength = expectedOffsets[implementLengthDomainId];
                var actualImplementLength = implementConfiguration.ImplementLength;

                AssertValue.VerifyNumericRepresentationValue(expectedImplementLength.Item1, expectedImplementLength.Item2, implementLengthDomainId, actualImplementLength);
            }

            var controlPointXOffsetDomainId = RepresentationInstanceList.vrInlineControlPointToConnectionOffset.DomainId;
            if (expectedOffsets.ContainsKey(controlPointXOffsetDomainId))
            {
                var expectedXOffset = expectedOffsets[controlPointXOffsetDomainId];
                var actualYOffset = implementConfiguration.ControlPoint.XOffset;

                AssertValue.VerifyNumericRepresentationValue(expectedXOffset.Item1, expectedXOffset.Item2, controlPointXOffsetDomainId, actualYOffset);
            }

            var controlPointYOffsetDomainId = RepresentationInstanceList.vrLateralControlPointToConnectionOffset.DomainId;
            if (expectedOffsets.ContainsKey(controlPointYOffsetDomainId))
            {
                var expectedYOffset = expectedOffsets[controlPointYOffsetDomainId];
                var acualYOffset = implementConfiguration.ControlPoint.YOffset;

                AssertValue.VerifyNumericRepresentationValue(expectedYOffset.Item1, expectedYOffset.Item2, controlPointYOffsetDomainId, acualYOffset);
            }

            var implementFrontOffsetDomainId = RepresentationInstanceList.vrImplementFrontOffset.DomainId;
            if (expectedOffsets.ContainsKey(implementFrontOffsetDomainId))
            {
                var expectedFrontOffset = expectedOffsets[implementFrontOffsetDomainId];
                var actualFrontOffset = implementConfiguration.Offsets.First(x=>x.Representation.Code == implementFrontOffsetDomainId);

                AssertValue.VerifyNumericRepresentationValue(expectedFrontOffset.Item1, expectedFrontOffset.Item2, implementFrontOffsetDomainId, actualFrontOffset);
            }

            var lateralConnectionPointToRecieverOffset = RepresentationInstanceList.vrLateralConnectionPointToReceiverOffset.DomainId;
            if (expectedOffsets.ContainsKey(lateralConnectionPointToRecieverOffset))
            {
                var expectedOffset = expectedOffsets[lateralConnectionPointToRecieverOffset];
                var actualOffset = implementConfiguration.Offsets.First(x => x.Representation.Code == lateralConnectionPointToRecieverOffset);

                AssertValue.VerifyNumericRepresentationValue(expectedOffset.Item1, expectedOffset.Item2, lateralConnectionPointToRecieverOffset, actualOffset);
            }

            var inlineConnectionPointToRecieverOffset = RepresentationInstanceList.vrInlineConnectionPointToReceiverOffset.DomainId;
            if (expectedOffsets.ContainsKey(inlineConnectionPointToRecieverOffset))
            {
                var expectedOffset = expectedOffsets[inlineConnectionPointToRecieverOffset];
                var actualOffset = implementConfiguration.Offsets.First(x => x.Representation.Code == inlineConnectionPointToRecieverOffset);

                AssertValue.VerifyNumericRepresentationValue(expectedOffset.Item1, expectedOffset.Item2, inlineConnectionPointToRecieverOffset, actualOffset);
            }
        }

        private static DeviceElementConfiguration VerifyDeviceElementConfiguration(List<Connector> catalogConnectors, List<DeviceElement> catalogDeviceElements,
            List<HitchPoint> catalogHitchPoints, List<DeviceElementConfiguration> catalogDeviceElementConfigurations, 
            string deviceElementDescription, HitchTypeEnum? expectedHitchTypeEnum)
        {
            var deviceElement = catalogDeviceElements.Find(x => x.Description == deviceElementDescription);
            Assert.IsNotNull(deviceElement);

            var deviceElementConfiguration = catalogDeviceElementConfigurations.Find(x => x.DeviceElementId == deviceElement.Id.ReferenceId);
            Assert.IsNotNull(deviceElementConfiguration);

            var connector = catalogConnectors.Find(x => x.DeviceElementConfigurationId == deviceElementConfiguration.Id.ReferenceId);
            Assert.IsNotNull(connector);

            if (connector.HitchPointId != 0 && expectedHitchTypeEnum != null)
            {
                var hitchPoint = catalogHitchPoints.Find(x => x.Id.ReferenceId == connector.HitchPointId);
                Assert.IsNotNull(hitchPoint);

                Assert.AreEqual(expectedHitchTypeEnum, hitchPoint.HitchTypeEnum);
            }

            return deviceElementConfiguration;
        }
    }
}
