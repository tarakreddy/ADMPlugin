using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgGateway.ADAPT.ADMPlugin;
using AgGateway.ADAPT.ApplicationDataModel.Equipment;
using AgGateway.ADAPT.ApplicationDataModel.Guidance;
using AgGateway.ADAPT.ApplicationDataModel.Logistics;
using AgGateway.ADAPT.ApplicationDataModel.Products;
using AgGateway.ADAPT.ApplicationDataModel.Shapes;
using AgGateway.ADAPT.Representation.RepresentationSystem;
using NUnit.Framework;
using AgGateway.ADAPT.TestUtilities;

namespace AgGateway.ADAPT.AcceptanceTest
{
    [TestFixture]
    public class CatalogAcceptanceTest
    {
        private string _cardPath;
        
        [Test]
        public void GivenOlderVersionSetupCardWhenImportedToVersionWithChangedPropertyAndTypeNamesThenImportsToTheNewerModel()
        {
            _cardPath = DatacardUtility.WriteDatacard("ADMSetupV1_0_8");

            var plugin = new Plugin();
            var applicationDataModel = plugin.Import(_cardPath);

            Assert.IsNotNull(applicationDataModel);

            var dataModel = applicationDataModel.First();
            var catalog = dataModel.Catalog;

            AssertLogistics.VerifyClient(catalog.Growers, "Client_1", "9309ad91-34a7-46d1-8fc4-2a8200bfd1fe");
            AssertLogistics.VerifyClient(catalog.Growers, "Client_2", "f012ab09-2bfa-4b43-bd84-0639fc964a37");
            AssertLogistics.VerifyClient(catalog.Growers, "Client_3", "c1453376-973a-4aae-afc7-e9cf124546b1");
            AssertLogistics.VerifyClient(catalog.Growers, "AccumulatedClient", "eef7e764-9b6e-490d-8650-c979ad0a6533");

            AssertLogistics.VerifyFarm(catalog.Farms, catalog.Growers, "Farm_1", "71695e14-c72a-4ad5-924e-6a8a93273b42", "Client_1");
            AssertLogistics.VerifyFarm(catalog.Farms, catalog.Growers, "Farm_2", "a9a27c6c-d535-49fe-9f7e-baa1be498dee", "Client_2");
            AssertLogistics.VerifyFarm(catalog.Farms, catalog.Growers, "Farm_3", "d084bb88-4df8-4fcb-a913-dd01b854cd78", "Client_3");
            AssertLogistics.VerifyFarm(catalog.Farms, catalog.Growers, "AccumulatedFarm", "46c3d57c-a87a-4324-a39d-d81a6547a1bc", "AccumulatedClient");

            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_1", "c5f76cad-2447-46bf-a291-64039bef531f", "Farm_1", 1000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_2", "1dbfd5b6-5e93-4c3b-a823-74194046714d", "Farm_2", 2000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_3", "1d427ee9-9530-48cd-bc09-610bc6aa0359", "Farm_3", 3000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_4", "fe0ef60f-cdbe-420e-af38-39511e31d3cb", "Farm_1", 1000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_5", "0bcbfbec-f064-42c2-a98a-57c434ba7be6", "Farm_1", 1000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_6", "65c1eb62-0812-48ff-89ab-c422907e1fb8", "Farm_1", 1000, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "AccumulatedField", "b23870b6-18de-4942-ac58-cf8671b87940", "AccumulatedFarm", 100, "ac");

            AssertLogistics.VerifyPerson(catalog.Persons, "Frank", "71661fa6-5cb8-4789-8e83-c9355a75aef6");
            AssertLogistics.VerifyPerson(catalog.Persons, "Lou", "d7802728-32ae-46e5-8ff4-ccf07fcd415d");
            AssertLogistics.VerifyPerson(catalog.Persons, "John", "8d5cb704-d41f-419f-abc0-ee7656ac2bf4");

            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Water", "ae248af1-d152-49c8-ab11-58ada63ec213", CategoryEnum.Unknown);

            // Defaults to additive when nothing is set. 
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "NH3", "93a50572-4b9a-4e17-8ea8-93883d544b6c", CategoryEnum.Additive); 
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "Manure", "7c548a23-66e8-40ae-a539-c571588a6b9c", CategoryEnum.Additive);

            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "Variety1", "140c9c02-6dae-429d-be46-4b9739a53cdb", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "Variety2", "6b9e76ef-6c46-4086-9eda-51d80911da8c", CategoryEnum.Variety, "Corn");

            AssertProducts.VerifyCrop(catalog.Crops, "Barley", "2", 21.77, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Canola", "5", 23.59, "kg1bu-1", 10, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Corn", "173", 25.4, "kg1bu-1", 15, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Cotton", "175", 480, "kg1bu-1", 6, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Oats", "11", 14.51, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Rape Seed", "16", 23.59, "kg1bu-1", 8, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Rice", "17", 20.41, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sorghum (milo)", "21", 25.4, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Soybeans", "174", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sunflowers", "40", 11.33, "kg1bu-1", 9, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat", "24", 27.22, "kg1bu-1", 13, "prcnt");

            AssertDevices.VerifyDeviceModel(catalog.DeviceModels, "9x30T", "5c3cab35-9829-42fe-932f-5c8b3331a8e4");
            AssertDevices.VerifyDeviceModel(catalog.DeviceModels, "7x30", "80bc222d-4dee-4bb1-897f-0af6e9ae2e9e");
            AssertDevices.VerifyDeviceModel(catalog.DeviceModels, "2410 Chisel Plow", "90c0810f-f492-485b-8ce3-179a8be45fbe");
            AssertDevices.VerifyDeviceModel(catalog.DeviceModels, "1770NT", "d143fca4-bad0-4281-b53e-8f93becd1230");

            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "8430", "serial-1", "d3ea4736-0424-47fd-9c93-e531362a1f4f", "Tractor", DeviceElementTypeEnum.Machine);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "8320", "serial-2", "fc979391-7cd8-45fe-905b-1d26aa96f91d", "Tractor", DeviceElementTypeEnum.Machine);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "4730", "serial-3", "dc562db9-a2ea-4420-a003-00a9ccbc8153", "Sprayer", DeviceElementTypeEnum.Machine);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "9560 STS", "serial-4", "67907d7f-014b-4625-81f6-a5a65f3e788a", "Combine", DeviceElementTypeEnum.Machine);

            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "373", "serial-1", "b4def480-ee2a-4a28-a6a1-3898af96c385", "Tillage", DeviceElementTypeEnum.Implement);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "1770 NT", "serial-1", "4135b823-13cf-41a9-8286-9eb40119800c", "Planter", DeviceElementTypeEnum.Implement);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "714 Tiller", "serial-2", "b9a50513-b9cd-4065-9373-ab73e8bfa1b6", "Tillage", DeviceElementTypeEnum.Implement);
            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "1890", "serial-3", "e89b3ac3-57bf-4510-80cd-5b2135e1da88", "Air Cart", DeviceElementTypeEnum.Implement);

            var offsets8430 = new Dictionary<string, Tuple<double, string>>
            {
                {RepresentationInstanceList.vrGPSToNonSteeringAxleOffset.DomainId, new Tuple<double, string>(4.7, "in")},
                {RepresentationInstanceList.vrReceiverOffset.DomainId, new Tuple<double, string>(9.5, "in")},
            };

            AssertDevices.VerifyMachineConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "8430", HitchTypeEnum.ISO730ThreePointHitchMounted, OriginAxleLocationEnum.Rear, offsets8430);
            AssertDevices.VerifyMachineConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "8320", HitchTypeEnum.ISO730ThreePointHitchMounted, OriginAxleLocationEnum.Rear, new Dictionary<string, Tuple<double, string>>());
            AssertDevices.VerifyMachineConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "4730", HitchTypeEnum.ISO730ThreePointHitchMounted, OriginAxleLocationEnum.Rear, new Dictionary<string, Tuple<double, string>>());
            AssertDevices.VerifyMachineConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "9560 STS", HitchTypeEnum.ISO730ThreePointHitchMounted, OriginAxleLocationEnum.Rear, new Dictionary<string, Tuple<double, string>>());

            var offsets373 = new Dictionary<string, Tuple<double, string>>
            {
                {RepresentationInstanceList.vrEquipmentWidth.DomainId, new Tuple<double, string>(32.0, "ft")},
                {RepresentationInstanceList.vrTrackSpacing.DomainId, new Tuple<double, string>(32.0, "ft")},
                {RepresentationInstanceList.vrPhysicalImplementWidth.DomainId, new Tuple<double, string>(33.0, "ft")},
                {RepresentationInstanceList.vrImplementLength.DomainId, new Tuple<double, string>(5.3, "ft")},
                {RepresentationInstanceList.vrInlineControlPointToConnectionOffset.DomainId, new Tuple<double, string>(9.8, "ft")},
                {RepresentationInstanceList.vrLateralControlPointToConnectionOffset.DomainId, new Tuple<double, string>(6.7, "in")},
                {RepresentationInstanceList.vrImplementFrontOffset.DomainId, new Tuple<double, string>(6.8, "ft")},
                {RepresentationInstanceList.vrLateralConnectionPointToReceiverOffset.DomainId, new Tuple<double, string>(0, "in")},
                {RepresentationInstanceList.vrInlineConnectionPointToReceiverOffset.DomainId, new Tuple<double, string>(0, "ft")}
            };

            AssertDevices.VerifyImplementConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "373", offsets373);
            AssertDevices.VerifyImplementConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "1770 NT", new Dictionary<string, Tuple<double, string>>());
            AssertDevices.VerifyImplementConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "714 Tiller", new Dictionary<string, Tuple<double, string>>());
            AssertDevices.VerifyImplementConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "1890", new Dictionary<string, Tuple<double, string>>());

            //This boundary data card does not have any goemetry attached to it. Check other tests which verify boundary spatial data
            AssertBoundaries.VerifyBoundary(catalog.FieldBoundaries, catalog.Fields, catalog.Farms, catalog.Growers,
                "36293da7-19b0-47dd-b28c-f42d2464ed5a", "Client_1", "9309ad91-34a7-46d1-8fc4-2a8200bfd1fe", 
                "Farm_1", "71695e14-c72a-4ad5-924e-6a8a93273b42", "Field_1", "c5f76cad-2447-46bf-a291-64039bef531f", 1000, "ac",
                GpsSourceEnum.DeereRTK, new List<string>{"Pond"}, 0);

            AssertGuidance.VerifyAbLine(catalog.GuidancePatterns, catalog.Growers, catalog.Farms, catalog.Fields,
                "ABLine1", "d3bb7e96-6f48-4fc5-b955-f9adef634189", "Client_2", "f012ab09-2bfa-4b43-bd84-0639fc964a37",
                "Farm_2", "a9a27c6c-d535-49fe-9f7e-baa1be498dee", "Field_2", "1dbfd5b6-5e93-4c3b-a823-74194046714d", 2000, "ac", 100.1, 0.1, 0.1, 
                new Point {X = -90.74156051, Y = 40.61723241}, new Point {X = -90.74162551, Y = 40.61084801});

            AssertGuidance.VerifyPivotGuidance(catalog.GuidancePatterns, catalog.Growers, catalog.Farms, catalog.Fields,
                "CircleTrack1", "5c257ad5-bcea-4ede-905c-6f286154f1a9", "Client_3", "c1453376-973a-4aae-afc7-e9cf124546b1",
                "Farm_3", "d084bb88-4df8-4fcb-a913-dd01b854cd78", "Field_3", "1d427ee9-9530-48cd-bc09-610bc6aa0359", 3000, "ac", 
                new Point { X = -90.49236844, Y = 41.48970819}, new Point { X = -90.49233529657748, Y = 41.489707467589085 }, 
                new Point { X = -90.49236844, Y = 41.48970819}, 0, "cm", PropagationDirectionEnum.NoPropagation, GuidanceExtensionEnum.None);

            //This abcurve data card does not have any goemetry attached to it. Check other tests which verify ab curve spatial data
            AssertGuidance.VerifyAbCurve(catalog.GuidancePatterns, catalog.Growers, catalog.Farms, catalog.Fields,
                "ABCurve1", "862db2bc-e7d8-4476-8aa6-f80830c77f3a", "Client_1", "9309ad91-34a7-46d1-8fc4-2a8200bfd1fe",
                "Farm_1", "71695e14-c72a-4ad5-924e-6a8a93273b42", "Field_5", "0bcbfbec-f064-42c2-a98a-57c434ba7be6", 1000, "ac", 9, 36.71572876, 0.0, 0.0,
                GpsSourceEnum.DesktopGeneratedData, new Point { X = -90.49276618, Y = 41.4895797 }, new Point { X = -90.49227984, Y = 41.49007002 }, 0, 0, 
                new List<Point>
                {
                    new Point{X =  -90.49331356, Y = 41.49024408},
                    new Point{X = -90.49173246, Y = 41.49024408},
                    new Point{X = -90.49173246, Y = 41.48941734},
                    new Point{X = -90.49331356, Y = 41.48941734},
                    new Point{X = -90.49331356, Y = 41.49024408}
                });
        }

        [Test]
        public void GivenOlderVersionDocCardWhenImportedToVersionWithChangedPropertyAndTypeNamesThenImportsToTheNewerModel()
        {
            _cardPath = DatacardUtility.WriteDatacard("ADMDocV1_0_8");

            var plugin = new Plugin();
            var applicationDataModel = plugin.Import(_cardPath);

            Assert.IsNotNull(applicationDataModel);

            var dataModel = applicationDataModel.First();
            var catalog = dataModel.Catalog;

            AssertLogistics.VerifyClient(catalog.Growers, "Client_1", "b470b6e9-5936-4d82-9514-a0fa06a94aa3");

            AssertLogistics.VerifyFarm(catalog.Farms, catalog.Growers, "Farm_1", "da829381-3984-4f17-b318-e432b2fa689e", "Client_1");

            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_1", "45ee819a-9e7e-49a1-b11b-0b54822b87a3", "Farm_1", 0, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_2", "872bcb5b-c1b2-49c2-81ba-b2645769385b", "Farm_1", 0, "ac");
            AssertLogistics.VerifyField(catalog.Fields, catalog.Farms, "Field_3", "d44acacb-5661-42d1-ac5e-f44008560048", "Farm_1", 0, "ac");

            AssertLogistics.VerifyPerson(catalog.Persons, "Mike", "4c0a7c43-66de-4fd5-bc97-96250f8d163b");

            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "10-34-0 Sol", "44d91e7a-3409-4dfd-877a-c250305093b0", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "28-0-0 UAN", "df4c35d6-c44b-4815-bef9-1b5904b4a842", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "32-0-0 UAN", "e767c7d2-ac9e-4059-8f23-34ea76509ed2", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Fertilizer (Dry)", "543a57c7-bd74-44a0-a240-8733c4d6410f", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Fertilizer (Liquid)", "f35b7f67-8c59-4303-bb41-28e84bee80cd", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Water", "53b03c91-d9e8-4d8e-a6dc-7e7ac43b8e87", CategoryEnum.Carrier);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Water and Fert", "fd859b30-20fa-41dc-a449-cf71ead27e99", CategoryEnum.Unknown);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "AMS", "3058764f-a429-4124-8b2e-07f6b9e3398d", CategoryEnum.Additive);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "COC", "58ff4894-6bec-4f3d-944b-83da800f9b75", CategoryEnum.Additive);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "MSO", "8c98defe-d955-4590-b091-7b5981cf7b64", CategoryEnum.Additive);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Bravo", "d0d6b57e-764d-4ac1-a19b-3c4d3bcd39d8", CategoryEnum.Fungicide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Captan", "6042e527-ee5b-4ad6-b3e1-c5e550637cb4", CategoryEnum.Fungicide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Atrazine", "57d16d1a-0df9-4615-b0b1-7b347d0f22bb", CategoryEnum.Herbicide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "RoundUp", "77421064-447c-4a2b-8dda-141d37cccf78", CategoryEnum.Herbicide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Select", "38c9cc0b-0364-4c85-9671-89b55f406897", CategoryEnum.Herbicide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Counter", "f62be802-bb95-4947-85da-4df296a17640", CategoryEnum.Insecticide);
            AssertProducts.VerifyCropProtectionProduct(catalog.Products, "Lorsban", "1d16391f-fd65-4ed9-bf2c-065a13e56cb7", CategoryEnum.Insecticide);

            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "10-34-0 Sol", "9702578d-50c1-420b-95d1-cc2d8d026446", CategoryEnum.Additive);
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "32-0-0 UAN", "c2e7949b-53b6-4edc-8827-b29b3a6a82ec", CategoryEnum.Additive);
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "Anhydrous Ammonia", "693a1402-8957-4c96-839d-fcbd050cc4a1", CategoryEnum.Additive);
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "BTN Plus", "4f3f002d-003c-4548-a6c4-f0e1d21f92d5", CategoryEnum.Additive);
            AssertProducts.VerifyCropNutrientProduct(catalog.Products, "ST 10-34-0 Mix", "3e451b8e-3ad5-4dc0-9962-0900545dc317", CategoryEnum.Additive);

            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "2750t", "44492e74-0000-1000-7fcf-e1e1e1000a60", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33B54", "6500dfa7-7887-45f9-ab96-884fd9e2c203", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33D11", "5aa640bd-be7d-44c4-8f07-02b66607cc07", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33H25", "441a8e1c-0b54-4662-920f-22c178732a62", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33K44", "d5ca0d5f-6897-4fb6-acf4-39c9b58fcfcc", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33N08", "55b1d3a4-d8bb-457a-bc74-2950701a6d71", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33N28", "a52ceafa-f012-41a7-b1d6-4fd8934a0dce", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "33P70", "539158fa-e19b-4620-909c-f215f11c5600", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34A14", "444785e5-0000-1000-7fc3-e1e1e1000a60", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34A16", "dde3a912-150b-42df-a152-780997b0d455", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34A17", "14718001-dcbe-48bc-bb65-f01c2fd76959", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34A18", "cc58d406-83d0-4ec1-9d1a-79884138925a", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34N42", "7b0cb813-94db-4061-bc9d-6a794e14bf85", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34N45", "3c18ffde-513c-4a84-b794-2feb73fb1b28", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "34N50", "e5b56a75-111d-45bd-ad4d-fb62e11fd37f", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "8H08", "444a31b7-0000-1000-7fe6-e1e1e1000a60", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "A6395", "876d0f7c-af0e-4c57-a54a-95e503b9c454", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "dkc49-94", "4dc46369-0000-1000-7fba-e1e1e124e078", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC52-62", "4bd6c368-0000-1000-4040-e1e1e1107bc8", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC54-46", "925b104c-5f01-489e-b287-644a8d37b91f", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "dkc55-24vt3votivo", "4dc49000-0000-1000-7fba-e1e1e124e078", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC60-18", "f53d46a1-9152-46c5-a7eb-90f41c781a36", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC60-19", "302bf3a0-0248-4ed6-aa7b-01f2341208ce", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC63-74", "0aa80a36-d5f8-47ae-9e69-59a57c42665c", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DKC63-81", "d9cf2d96-254d-4c93-8e4e-f987af987755", CategoryEnum.Variety, "Corn");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "DP 555", "95f33c7b-7fc1-4674-8946-ff726f70932d", CategoryEnum.Variety, "Cotton");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "2932", "445cc9fc-0000-1000-7fdf-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "92M91", "446b03a2-0000-1000-4003-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "93M11", "bb32538a-9998-4961-9d16-304e8ae0c8c6", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "93M20", "1bcd6846-9d67-4608-8dd6-3b28bc0de431", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "H-2162", "a17b0c79-b2ed-4e69-a9c9-43f9ce731033", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "H-2162-N", "eeb473d5-17c1-4b6d-b5fd-a9803be7e0b7", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "mixed", "4460f0be-0000-1000-7fe4-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "R2633", "44606a5c-0000-1000-7fe4-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "R3143", "44624c87-0000-1000-7fe7-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");
            AssertProducts.VerifyCropVariety(catalog.Products, catalog.Crops, "RV2509", "44686019-0000-1000-7fff-e1e1e10d6e40", CategoryEnum.Variety, "Soybeans");

            AssertProducts.VerifyCrop(catalog.Crops, "Barley", "2", 21.77, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Canola", "5", 23.59, "kg1bu-1", 10, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Corn", "173", 25.4, "kg1bu-1", 15, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Oats", "11", 14.51, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Rape Seed", "16", 23.59, "kg1bu-1", 10, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Cotton", "175", 480, "lb1bale-1", 1, "prcnt", "vrCropWeightBale");
            AssertProducts.VerifyCrop(catalog.Crops, "Rice (Long)", "17", 20.41, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Rice (Medium)", "18", 20.41, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sorghum", "21", 25.4, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Soybeans", "174", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sunflower (E Ind)", "40", 11.33, "kg1bu-1", 9, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sunflower (E Oil)", "41", 11.33, "kg1bu-1", 9, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sunflower (Oil)", "22", 13.14, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Sunflower (Stripe)", "23", 9.07, "kg1bu-1", 14, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Durum)", "24", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Euro Feed)", "43", 27.22, "kg1bu-1", 14.50, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Euro Wtr)", "44", 27.22, "kg1bu-1", 14.50, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Hrd Rd Spr)", "25", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Hrd Rd Wtr)", "26", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (Sft Rd Wtr)", "27", 27.22, "kg1bu-1", 13, "prcnt");
            AssertProducts.VerifyCrop(catalog.Crops, "Wheat (White)", "28", 27.22, "kg1bu-1", 13, "prcnt");

            AssertProducts.VerifyProductMix(catalog.Products, "App #2 Tank Mix",
                "a02816bb-a66f-4154-94b5-c74dc7fbebdb", "53b03c91-d9e8-4d8e-a6dc-7e7ac43b8e87", "Water", 32, "floz1ac-1", "vrSolutionRateLiquid" ,CategoryEnum.Additive,
                new List<Tuple<string, double, string,string>>
                {
                    new Tuple<string, double, string,string>("Bravo", 8, "floz1ac-1","vrSolutionRateLiquid")
                });

            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "9660 STS", " ", "50a0d697-2173-4a18-994e-b91b3b735332", "Combine", DeviceElementTypeEnum.Machine);

            AssertDevices.VerifyDeviceElement(catalog.DeviceElements, "12 Row Corn Head", "H01293X706019", "5c2dd3ae-8282-46a9-9ad3-5cf83ac1b8c0", "Corn Head", DeviceElementTypeEnum.Implement);

            AssertDevices.VerifyMachineConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "9660 STS", HitchTypeEnum.ISO730ThreePointHitchMounted, OriginAxleLocationEnum.Rear, new Dictionary<string, Tuple<double, string>>());

            AssertDevices.VerifyImplementConfiguration(catalog.Connectors, catalog.DeviceElements, catalog.DeviceElementConfigurations, catalog.HitchPoints,
                "12 Row Corn Head", new Dictionary<string, Tuple<double, string>>());            
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                var directoryName = Path.GetDirectoryName(_cardPath);
                if (directoryName != null)
                {
                    Directory.Delete(directoryName, true);
                }
            }
            catch { }
        }
    }
}
