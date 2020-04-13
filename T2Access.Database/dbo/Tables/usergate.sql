CREATE TABLE [dbo].[usergate] (
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [GateId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         CONSTRAINT [DF__usergate__Create__6C190EBB] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_usergate_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC, [GateId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [FK_UserGate_Gate]
    ON [dbo].[usergate]([GateId] ASC);


GO
ALTER INDEX [FK_UserGate_Gate]
    ON [dbo].[usergate] DISABLE;


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.usergate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'usergate';

