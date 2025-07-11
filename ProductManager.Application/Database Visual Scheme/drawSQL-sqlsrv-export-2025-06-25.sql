CREATE TABLE "Categories"(
    "CategoryID" BIGINT NOT NULL,
    "CategoryName" NVARCHAR(255) NOT NULL
);
ALTER TABLE
    "Categories" ADD CONSTRAINT "categories_categoryid_primary" PRIMARY KEY("CategoryID");
CREATE TABLE "Products"(
    "ProductID" BIGINT NOT NULL,
    "ProductName" VARCHAR(255) NOT NULL,
    "ProductDescription" VARCHAR(255) NOT NULL,
    "CategoryID" BIGINT NULL,
    "ProductPrice" DECIMAL(8, 2) NOT NULL,
    "ProductQuantityID" BIGINT NULL
);
ALTER TABLE
    "Products" ADD CONSTRAINT "products_productid_primary" PRIMARY KEY("ProductID");
CREATE TABLE "Pricelists"(
    "PricelistID" BIGINT NOT NULL,
    "PricelistName" VARCHAR(255) NOT NULL,
    "PriceListDateCreation" DATETIME NOT NULL,
    "PriceListDateModification" DATETIME NOT NULL
);
ALTER TABLE
    "Pricelists" ADD CONSTRAINT "pricelists_pricelistid_primary" PRIMARY KEY("PricelistID");
CREATE TABLE "PricelistProductPurchases"(
    "PurchaseID" BIGINT NOT NULL,
    "PricelistID" BIGINT NULL,
    "ProductID" BIGINT NULL,
    "ProductQuantityNumber" FLOAT(53) NOT NULL,
    "ProductPriceAtBuy" DECIMAL(8, 2) NOT NULL,
    "ProductQuantityNameAtBuy" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "PricelistProductPurchases" ADD CONSTRAINT "pricelistproductpurchases_purchaseid_primary" PRIMARY KEY("PurchaseID");
CREATE TABLE "OptionalParameters"(
    "OptionalParameterID" BIGINT NOT NULL,
    "OptionalParameterType" VARCHAR(255) NOT NULL,
    "OptionalParameterName" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "OptionalParameters" ADD CONSTRAINT "optionalparameters_optionalparameterid_primary" PRIMARY KEY("OptionalParameterID");
CREATE TABLE "PricelistOptionalParameters"(
    "OptionalParameterEntryID" BIGINT NOT NULL,
    "OptionalParameterID" BIGINT NULL,
    "OptionalParameterValue" BIGINT NOT NULL,
    "PurchaseID" BIGINT NULL
);
ALTER TABLE
    "PricelistOptionalParameters" ADD CONSTRAINT "pricelistoptionalparameters_optionalparameterentryid_primary" PRIMARY KEY("OptionalParameterEntryID");
CREATE TABLE "ProductQuantities"(
    "ProductQuantityID" BIGINT NOT NULL,
    "ProductQuantityName" VARCHAR(255) NOT NULL,
    "ProductQuantityTypeID" BIGINT NULL
);
ALTER TABLE
    "ProductQuantities" ADD CONSTRAINT "productquantities_productquantityid_primary" PRIMARY KEY("ProductQuantityID");
CREATE TABLE "ProductQuantityTypes"(
    "ProductQuantityTypeID" BIGINT NOT NULL,
    "ProductQuantityTypeName" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "ProductQuantityTypes" ADD CONSTRAINT "productquantitytypes_productquantitytypeid_primary" PRIMARY KEY("ProductQuantityTypeID");
ALTER TABLE
    "ProductQuantities" ADD CONSTRAINT "productquantities_productquantitytypeid_foreign" FOREIGN KEY("ProductQuantityTypeID") REFERENCES "ProductQuantityTypes"("ProductQuantityTypeID");
ALTER TABLE
    "PricelistOptionalParameters" ADD CONSTRAINT "pricelistoptionalparameters_optionalparameterid_foreign" FOREIGN KEY("OptionalParameterID") REFERENCES "OptionalParameters"("OptionalParameterID");
ALTER TABLE
    "PricelistOptionalParameters" ADD CONSTRAINT "pricelistoptionalparameters_purchaseid_foreign" FOREIGN KEY("PurchaseID") REFERENCES "PricelistProductPurchases"("PurchaseID");
ALTER TABLE
    "Products" ADD CONSTRAINT "products_productquantityid_foreign" FOREIGN KEY("ProductQuantityID") REFERENCES "ProductQuantities"("ProductQuantityID");
ALTER TABLE
    "PricelistProductPurchases" ADD CONSTRAINT "pricelistproductpurchases_pricelistid_foreign" FOREIGN KEY("PricelistID") REFERENCES "Pricelists"("PricelistID");
ALTER TABLE
    "Products" ADD CONSTRAINT "products_categoryid_foreign" FOREIGN KEY("CategoryID") REFERENCES "Categories"("CategoryID");
ALTER TABLE
    "PricelistProductPurchases" ADD CONSTRAINT "pricelistproductpurchases_productid_foreign" FOREIGN KEY("ProductID") REFERENCES "Products"("ProductID");