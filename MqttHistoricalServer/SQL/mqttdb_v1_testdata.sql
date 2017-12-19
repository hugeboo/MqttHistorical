USE [mqttdb]

INSERT INTO Users ([Name], [Password], [Enabled]) VALUES ('testuser', '12345678', 1)
GO

INSERT INTO Connections ([UserId], [Server], [ConnectionUser], [Password], [Port], [SSLPort], [UseSSL], [Enabled]) VALUES
	(1, 'm14.cloudmqtt.com', 'sreuxfkx', 'UCImiG1bEvxX', 13715, 23715, 0, 1)
GO

INSERT INTO Subscriptions ([ConnectionId], [TopicFilter], [QoS], [Enabled]) VALUES (1, '*/node/#', 0, 1)
INSERT INTO Subscriptions ([ConnectionId], [TopicFilter], [QoS], [Enabled]) VALUES (1, 'E9D4C0/tempc', 0, 1)
GO

INSERT INTO Topics ([ConnectionId], [Name]) VALUES (1, 'E9D4C0/node/test')
GO

INSERT INTO Payloads ([TopicId], [Timestamp], [Data]) VALUES (1, 1, 'data number one')
GO
