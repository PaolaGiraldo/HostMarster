SET IDENTITY_INSERT [dbo].[Reservations] ON;

INSERT [dbo].[Reservations] ([Id], [StartDate], [EndDate], [NumberOfGuests], [ReservationState], [RoomId], [CustomerId], [AccommodationId]) 
VALUES 
    (1, N'2024-09-28 14:30:00', N'2024-09-30 09:00:00', 2, N'Confirmed', 1, 1, 1),
    (2, N'2024-10-01 18:15:00', N'2024-10-02 12:45:00', 4, N'Pending', 2, 1, 1),
    (3, N'2024-10-03 08:00:00', N'2024-10-04 22:10:00', 1, N'Canceled', 3, 3, 1);

SET IDENTITY_INSERT [dbo].[Reservations] OFF;