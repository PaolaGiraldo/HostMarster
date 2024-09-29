SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT [dbo].[Users] ([Id], [Email], [DocumentType], [Document], [FirstName], [LastName], [Photo], [PhoneNumber], [UserType]) 
VALUES 
    (1, 'john.doe@example.com', 'CC', '123456789', 'John', 'Doe', NULL, '555-0101', 0),
    (2, 'jane.smith@example.com', 'CC', '987654321', 'Jane', 'Smith', NULL, '555-0202', 1),
    (3, 'emily.johnson@example.com', 'CC', '111223344', 'Emily', 'Johnson', NULL, '555-0303', 0), 
    (4, 'michael.brown@example.com', 'TI', '222334455', 'Michael', 'Brown', NULL, '555-0404', 1),
    (5, 'sarah.wilson@example.com', 'CC', '333445566', 'Sarah', 'Wilson', NULL, '555-0505', 1); 

SET IDENTITY_INSERT [dbo].[Users] OFF;