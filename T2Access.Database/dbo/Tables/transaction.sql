CREATE TABLE [dbo].[transaction] (
    [Id]          DECIMAL (18)     IDENTITY (7, 1) NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [GateId]      UNIQUEIDENTIFIER NOT NULL,
    [Status]      INT              CONSTRAINT [DF__transacti__Statu__6754599E] DEFAULT ((0)) NOT NULL,
    [StatusDate]  DATETIME         CONSTRAINT [DF__transacti__Statu__68487DD7] DEFAULT (getdate()) NULL,
    [CreatedDate] DATETIME         CONSTRAINT [DF__transacti__Creat__693CA210] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_transaction_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [transaction$Id_UNIQUE] UNIQUE NONCLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [FK_Transaction_Gate]
    ON [dbo].[transaction]([GateId] ASC);


GO
ALTER INDEX [FK_Transaction_Gate]
    ON [dbo].[transaction] DISABLE;


GO
CREATE NONCLUSTERED INDEX [FK_Transaction_User]
    ON [dbo].[transaction]([UserId] ASC);


GO
ALTER INDEX [FK_Transaction_User]
    ON [dbo].[transaction] DISABLE;


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.transaction', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'transaction';

