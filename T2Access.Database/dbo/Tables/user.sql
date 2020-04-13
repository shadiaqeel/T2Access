CREATE TABLE [dbo].[user] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Username]    NVARCHAR (255)   NOT NULL,
    [Password]    NVARCHAR (255)   NOT NULL,
    [Firstname]   NVARCHAR (255)   NOT NULL,
    [Lastname]    NVARCHAR (255)   NOT NULL,
    [CreatedDate] DATETIME         CONSTRAINT [DF__user__CreatedDat__6A30C649] DEFAULT (getdate()) NOT NULL,
    [Status]      INT              CONSTRAINT [DF__user__Status__6B24EA82] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_user_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [user$Id_UNIQUE] UNIQUE NONCLUSTERED ([Id] ASC),
    CONSTRAINT [user$Username_UNIQUE] UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.`user`', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'user';

