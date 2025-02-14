/**
Таблица категорий продуктов.
*/
CREATE TABLE [Categories] (
	-- ИД категории продукта.
	[CategoryID] BIGINT NOT NULL UNIQUE,
	-- Название категории продукта.
	[CategoryName] NVARCHAR NOT NULL,
	PRIMARY KEY([CategoryID])
);
GO

/**
Таблица продуктов.
*/
CREATE TABLE [Products] (
	-- ИД продукта.
	[ProductID] BIGINT NOT NULL UNIQUE,
	-- Название продукта.
	[ProductName] NVARCHAR NOT NULL,
	-- Описание продукта.
	[ProductDescription] NVARCHAR NOT NULL,
	-- Ид категории, данного продукта.
	[CategoryID] BIGINT NOT NULL,
	-- Цена продукта.
	[ProductPrice] MONEY NOT NULL DEFAULT 0.00,
	PRIMARY KEY([ProductID])
);
GO

/**
Таблица покупок определённого количества продуктов, по некоторой цене, в указанном прайслисте.
*/
CREATE TABLE [PriceListProductPurchases] (
	-- ИД покупки некоторого количества продукта с определённой ценой.
	[PurchaseID] BIGINT NOT NULL UNIQUE,
	-- ИД прайс листа к которому относится данная покупка.
	[PriceListID] BIGINT NOT NULL,
	-- ИД продукта, который купили.
	[ProductID] BIGINT NOT NULL,
	-- Количество продукта, которое было куплено.
	[ProductQuantity] INTEGER NOT NULL,
	-- Цена, за которую был приобретён продукт на момент оформления прайслиста.
	[ProductPrice] MONEY NOT NULL DEFAULT 0.00,
	PRIMARY KEY([PurchaseID])
);
GO

/**
Таблица прайслистов.
*/
CREATE TABLE [PriceLists] (
	-- ИД прайслиста.
	[PriceListID] BIGINT NOT NULL UNIQUE,
	-- Название прайслиста.
	[PriceListName] NVARCHAR NOT NULL,
	-- Дата и время создание прайслиста.
	[PriceListDateCreation] DATETIME2 NOT NULL,
	PRIMARY KEY([PriceListID])
);
GO

/**
Таблица вхождений опциональных параметров со значениями для конкретного прайслиста.
*/
CREATE TABLE [PriceListOptionalParameters] (
	-- ИД конкретного опционального параметра для данного прайслиста с его значением. 
	[OptionalParameterEntryID] BIGINT NOT NULL UNIQUE,
	-- ИД опционального параметра для данного прайслиста.
	[OptionalParameterID] BIGINT NOT NULL,
	-- Значение опционального параметра для данного прайслиста.
	[OptionalParameterValue] NVARCHAR NOT NULL,
	-- ИД прайслиста, для которого указывается опциональный параметр со значением.
	[PriceListID] BIGINT NOT NULL,
	PRIMARY KEY([OptionalParameterEntryID])
);
GO

/**
Таблица опциональных параметров с именами и их типами.
*/
CREATE TABLE [OptionalParameters] (
	-- ИД опционального параметра.
	[OptionalParameterID] BIGINT NOT NULL UNIQUE,
	-- Тип опционального параметра.
	[OptionalParameterType] NVARCHAR NOT NULL,
	-- Название опционального параметра.
	[OptionalParameterName] NVARCHAR NOT NULL,
	PRIMARY KEY([OptionalParameterID])
);
GO

ALTER TABLE [Categories]
ADD FOREIGN KEY([CategoryID]) REFERENCES [Products]([CategoryID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Products]
ADD FOREIGN KEY([ProductID]) REFERENCES [PriceListProductPurchases]([ProductID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [PriceLists]
ADD FOREIGN KEY([PriceListID]) REFERENCES [PriceListProductPurchases]([PriceListID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [PriceLists]
ADD FOREIGN KEY([PriceListID]) REFERENCES [PriceListOptionalParameters]([PriceListID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OptionalParameters]
ADD FOREIGN KEY([OptionalParameterID]) REFERENCES [PriceListOptionalParameters]([OptionalParameterID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO