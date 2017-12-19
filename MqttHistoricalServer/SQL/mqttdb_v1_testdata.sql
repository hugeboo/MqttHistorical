USE [mqttdb]

INSERT INTO Users ([Name], [Password]) VALUES ('testuser', '12345678')
GO

INSERT INTO Connections ([UserId], [Server], [ConnectionUser], [Password], [Port], [SSLPort], [UseSSL]) VALUES
	(1, 'm14.cloudmqtt.com', 'sreuxfkx', 'UCImiG1bEvxX', 13715, 23715, 0)
GO

INSERT INTO Subscriptions ([ConnectionId], [TopicFilter], [QoS]) VALUES (1, '*/node/#', 0)
INSERT INTO Subscriptions ([ConnectionId], [TopicFilter], [QoS]) VALUES (1, 'E9D4C0/tempc', 0)
GO

INSERT INTO Topics ([ConnectionId], [Name]) VALUES (1, 'E9D4C0/node/test')
GO

INSERT INTO Payloads ([TopicId], [Timestamp], [Data]) VALUES (1, 1, 'data number one')
GO
