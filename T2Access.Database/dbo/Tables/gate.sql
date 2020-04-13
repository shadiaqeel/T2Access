CREATE TABLE [dbo].[gate] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [NameAr]      NVARCHAR (255)   CONSTRAINT [DF__gate__NameAr__6383C8BA] DEFAULT (NULL) NULL,
    [NameEn]      NVARCHAR (255)   CONSTRAINT [DF__gate__NameEn__6477ECF3] DEFAULT (NULL) NULL,
    [Status]      INT              CONSTRAINT [DF__gate__Status__656C112C] DEFAULT ((0)) NOT NULL,
    [CreatedDate] DATETIME         CONSTRAINT [DF__gate__CreatedDat__66603565] DEFAULT (getdate()) NOT NULL,
    [Username]    NVARCHAR (255)   NOT NULL,
    [Password]    NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_gate_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [gate$Id_UNIQUE] UNIQUE NONCLUSTERED ([Id] ASC),
    CONSTRAINT [gate$Username_UNIQUE] UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.gate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'gate';

