﻿SET IDENTITY_INSERT [dbo].[Reservations] ON;

INSERT [dbo].[Reservations] ([Id], [StartDate], [EndDate], [NumberOfGuests], [ReservationState], [RoomId], [CustomerDocumentNumber], [AccommodationId]) 
VALUES 
     (1, N'2024-09-28 14:30:00', N'2024-09-30 09:00:00', 2, N'Confirmed', 1, 123456789, 1),
    (2, N'2024-10-01 18:15:00', N'2024-10-02 12:45:00', 4, N'Pending', 2, 987654321, 1),
    (3, N'2024-10-03 08:00:00', N'2024-10-04 22:10:00', 1, N'Canceled', 3, 987654321, 1),
    (4, N'2024-10-05 15:00:00', N'2024-10-07 11:00:00', 3, N'Confirmed', 1, 123456780, 1),
    (5, N'2024-10-10 09:00:00', N'2024-10-12 17:00:00', 2, N'Confirmed', 2, 234567890, 1),
    (6, N'2024-10-15 14:00:00', N'2024-10-18 10:00:00', 5, N'Pending', 3, 345678901, 1),
    (7, N'2024-10-20 16:30:00', N'2024-10-22 09:30:00', 4, N'Confirmed', 1, 456789012, 1),
    (8, N'2024-10-23 17:00:00', N'2024-10-25 12:00:00', 6, N'Canceled', 2, 567890123, 1),
    (9, N'2024-10-28 13:00:00', N'2024-10-30 11:00:00', 3, N'Confirmed', 3, 678901234, 1),
    (10, N'2024-11-01 18:00:00', N'2024-11-03 10:00:00', 2, N'Pending', 2, 789012345, 1),
    (11, N'2024-11-05 09:30:00', N'2024-11-07 16:30:00', 1, N'Confirmed', 1, 890123456, 1),
    (12, N'2024-11-10 15:00:00', N'2024-11-12 12:00:00', 4, N'Confirmed', 2, 901234567, 1),
    (13, N'2024-11-15 14:30:00', N'2024-11-17 09:00:00', 5, N'Pending', 3, 123456789, 1),
    (14, N'2024-11-20 16:15:00', N'2024-11-22 10:45:00', 2, N'Confirmed', 1, 234567890, 1),
    (15, N'2024-11-25 18:00:00', N'2024-11-27 11:30:00', 3, N'Confirmed', 2, 345678901, 1),
    (16, N'2024-12-01 14:30:00', N'2024-12-03 09:00:00', 2, N'Confirmed', 1, 123456789, 1),
    (17, N'2024-12-04 18:15:00', N'2024-12-06 12:45:00', 4, N'Pending', 2, 987654321, 1),
    (18, N'2024-12-08 08:00:00', N'2024-12-10 22:10:00', 1, N'Canceled', 3, 111223344, 1),
    (19, N'2024-12-12 15:00:00', N'2024-12-15 11:00:00', 3, N'Confirmed', 1, 123456780, 1),
    (20, N'2024-12-16 09:00:00', N'2024-12-18 17:00:00', 2, N'Confirmed', 2, 234567890, 1),
    (21, N'2024-12-20 14:00:00', N'2024-12-22 10:00:00', 5, N'Pending', 3, 345678901, 1);


SET IDENTITY_INSERT [dbo].[Reservations] OFF;