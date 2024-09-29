SET IDENTITY_INSERT [dbo].[Customers] ON;

INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [DocumentType], [DocumentNumber], [Email], [PhoneNumber]) 
VALUES 
    (1, N'John', N'Doe', N'CC', 123456789, N'john.doe@example.com', N'555-1234'),
    (2, N'Jane', N'Smith', N'CC', 987654321, N'jane.smith@example.com', N'555-5678'),
    (3, N'Emily', N'Johnson', N'CC', 111223344, N'emily.johnson@example.com', N'555-8765');

SET IDENTITY_INSERT [dbo].[Customers] OFF;