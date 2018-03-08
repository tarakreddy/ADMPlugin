using System;
using System.Collections.Generic;
using System.Linq;
using AgGateway.ADAPT.ApplicationDataModel.Products;
using AgGateway.ADAPT.Representation.RepresentationSystem;
using NUnit.Framework;

namespace AgGateway.ADAPT.AcceptanceTest
{
    public static class AssertProducts
    {
        public static void VerifyCropProtectionProduct(List<Product> catalogProducts, string expectedDescription, string expectedGuid, CategoryEnum expectedCategoryEnum)
        {
            VerifyProduct(catalogProducts, expectedDescription, expectedGuid, expectedCategoryEnum, typeof(CropProtectionProduct));
        }

        public static void VerifyCropNutrientProduct(List<Product> catalogProducts, string expectedDescription, string expectedGuid, CategoryEnum expectedCategoryEnum)
        {
            VerifyProduct(catalogProducts, expectedDescription, expectedGuid, expectedCategoryEnum, typeof(CropNutritionProduct));
        }

        public static void VerifyCropVariety(List<Product> catalogProducts, List<Crop> catalogCrops, string expectedDescription, 
            string expectedGuid, CategoryEnum expectedCategoryEnum, string expectedCrop)
        {
            var product = VerifyProduct(catalogProducts, expectedDescription, expectedGuid, expectedCategoryEnum, typeof(CropVarietyProduct));

            var crop = catalogCrops.Find(x => x.Name == expectedCrop);
            Assert.AreEqual(crop.Id.ReferenceId, ((CropVarietyProduct)product).CropId);
        }

        private static Product VerifyProduct(List<Product> catalogProducts, string expectedDescription, string expectedGuid,
            CategoryEnum expectedCategoryEnum, Type instanceType)
        {
            var product = catalogProducts.Find(x => x.Id.UniqueIds.Exists(id => id.Id == expectedGuid));

            Assert.IsNotNull(product);
            Assert.IsInstanceOf(instanceType, product);
            Assert.AreEqual(expectedDescription, product.Description);
            Assert.AreEqual(expectedCategoryEnum, product.Category);
            return product;
        }

        public static void VerifyCrop(List<Crop> catalogCrops, string expectedName, string expectedId, 
            double expectedCropWeight, string expectedCropWeightUnit, 
            double expectedStandardPayableMoisture, string expectedStandardPayableMoistureUnit, string variableRepresentation = "vrCropWeightVolume")
        {
            var crop = catalogCrops.Find(x => x.Id.UniqueIds.First().Id == expectedId);

            Assert.IsNotNull(crop);
            Assert.AreEqual(expectedName, crop.Name);

            AssertValue.VerifyNumericRepresentationValue(expectedCropWeight, expectedCropWeightUnit, variableRepresentation, crop.ReferenceWeight);

            AssertValue.VerifyNumericRepresentationValue(expectedStandardPayableMoisture, expectedStandardPayableMoistureUnit,
                RepresentationInstanceList.vrStandardPayableMoisture.DomainId, crop.StandardPayableMoisture);
        }

        public static void VerifyProductMix(List<Product> catalogProducts,
            string expectedDescription, string expectedGuid, string expectedProductMixCarrierId,
            string expectedCarrierDescription, double expectedProductMixSolutionRate, string expectedProductMixSolutionUnit,string expectedProductMixVariableRepresentation,
            CategoryEnum expectedCategoryEnum, List<Tuple<string, double, string,string>> components)
        {
            var carrier = VerifyProduct(catalogProducts, expectedCarrierDescription, expectedProductMixCarrierId, CategoryEnum.Carrier,
                typeof(Product));
            Assert.IsNotNull(carrier);

            var actualProduct = VerifyProduct(catalogProducts, expectedDescription, expectedGuid, expectedCategoryEnum, typeof(MixProduct));
            var actualMixProduct = (MixProduct)actualProduct;

            AssertValue.VerifyNumericRepresentationValue(expectedProductMixSolutionRate, expectedProductMixSolutionUnit, expectedProductMixVariableRepresentation, actualMixProduct.TotalQuantity);

            foreach (var expectedComponent in components)
            {
                var actualIngredient = catalogProducts.Find(x => x.Description == expectedComponent.Item1);
                Assert.IsNotNull(actualIngredient);

                var actualProductComponent = actualMixProduct.ProductComponents.Find(x => x.IngredientId == actualIngredient.Id.ReferenceId);
                AssertValue.VerifyNumericRepresentationValue(expectedComponent.Item2, expectedComponent.Item3,
                    expectedComponent.Item4, actualProductComponent.Quantity);
                    
            }
        }
    }
}
