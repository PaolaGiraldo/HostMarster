SET IDENTITY_INSERT [dbo].[RoomTypes] ON 
INSERT [dbo].[RoomTypes] ([Id], [TypeName],[Description],[Price],[MaxGuests]) VALUES (1, N'SINGLE',N'Una cama sencilla',60.00,2)
INSERT [dbo].[RoomTypes] ([Id], [TypeName],[Description],[Price],[MaxGuests]) VALUES (2, N'DOUBLE',N'Una cama doble',80.00,2)
INSERT [dbo].[RoomTypes] ([Id], [TypeName],[Description],[Price],[MaxGuests]) VALUES (3, N'DOUBLE 2 BEDS',N'Dos camas dobles',80.00,4)
SET IDENTITY_INSERT [dbo].[RoomTypes] OFF