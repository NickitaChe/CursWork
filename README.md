# DB

Решение является моей курсовой работой за 5 семестор в университете Лэти по дисциплине Управление Данными  
The solution is my 5th semester coursework at LETI University in the discipline of Data Management  

## Описание / Description
Для реализации проекта был выбран язык C# - как один из самых быстро развивающихся языков для объектно ориентированного программирования.
Поскольку приложение разрабатывается под Windows выбран фреймворк .NET 
Как самый легко подключаемый к данному фреймворку СУБД был выбран MSSQL.



## Задание / Task
Спроектировать базу данных, построить программу, обеспечивающую взаимодействие с ней в режиме диалога, для продавца в магазине. В БД хронятся сведения о наличии товара, информация о сотрудниках, кассовые чеки, данные клиентов.
Каждый сотрудник: ФИО, телефон, E-Mail, комиссия. По умолчанию продавец получает 15% от дохода магазина.
Каждый чек содержит информацию о товарах в этом чеке, их количестве, информацию о продавце, и клиенте, а также дату совершения покупки.
У сотрудника есть возможность принимать товар на склад, а также добавлять новые позиции.
Магазин должен вести статистику, о зарплатах сотрудника за месяц, доходности магазина по дням, и месяцам. О самых востребованных товарах, о товарах, которые почти закончились на складе.
Также должна быть возможность узнать самого эффективного сотрудника, самый доходный месяц и день.




## Настройка / installation

### Создание Таблиц в Базе данных / Creating Tables in a Database
``` sql
CREATE TABLE [dbo].[Costumers] (
    [ID]      INT        IDENTITY (1, 1) NOT NULL,
    [Name]    NCHAR (40) NULL,
    [Surname] NCHAR (40) NULL,
    [Telnum]  NCHAR (40) NULL,
    [Email]   NCHAR (40) DEFAULT (N'none') NULL,
    CONSTRAINT [PK_Costumers] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[SellChek] (
    [ID]          INT  IDENTITY (1, 1) NOT NULL,
    [Unit_Id]     INT  NOT NULL,
    [Costumer_Id] INT  NOT NULL,
    [Seller_Id]   INT  NOT NULL,
    [Quantity]    INT  NOT NULL,
    [Sell_numb]   INT  NOT NULL,
    [Date]        DATE NULL,
    CONSTRAINT [PK_SellChek] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SellChek_Costumers] FOREIGN KEY ([Costumer_Id]) REFERENCES [dbo].[Costumers] ([ID]),
    CONSTRAINT [FK_SellChek_Units] FOREIGN KEY ([Unit_Id]) REFERENCES [dbo].[Units] ([ID]),
    CONSTRAINT [FK_SellChek_Sellers] FOREIGN KEY ([Seller_Id]) REFERENCES [dbo].[Sellers] ([ID])
);

CREATE TABLE [dbo].[Sellers] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]      NCHAR (50)      NOT NULL,
    [Surname]   NCHAR (50)      NOT NULL,
    [Telnum]    NCHAR (50)      NULL,
    [Email]     NCHAR (50)      NULL,
    [Commision] DECIMAL (18, 2) NOT NULL,
    [Login]     NCHAR (50)      NOT NULL,
    [Password]  NCHAR (50)      NOT NULL,
    CONSTRAINT [PK_Sellers] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Units] (
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [Unit_name]           NCHAR (45)    NOT NULL,
    [Unit_of_measurement] NVARCHAR (50) NULL,
    [BUY_Price]           INT           NOT NULL,
    [SELL_Price]          INT           NOT NULL,
    [Quantity]            INT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Units]
    ON [dbo].[Units]([ID] ASC);

```

### Настройка конфига / Config setup (App.config)

```html
<connectionStrings>
    <add name ="DBKey" connectionString=""/> // Сюда нужно поместить ссылку на базу данных
</connectionStrings>
```
