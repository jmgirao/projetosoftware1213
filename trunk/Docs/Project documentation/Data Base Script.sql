IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_Shortcut_Task') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE Shortcut DROP CONSTRAINT FK_Shortcut_Task;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_Time_Task') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE TaskTime DROP CONSTRAINT FK_Time_Task;



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Configuration') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Configuration;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Shortcut') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Shortcut;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Task') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Task;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('TaskTime') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE TaskTime;


CREATE TABLE Configuration ( 
	InactivityEnabled bit NOT NULL,
	InactivityTime tinyint
);

CREATE TABLE Shortcut ( 
	ShortcutId tinyint NOT NULL,
	TaskId bigint NOT NULL,
	Ctrl bit NOT NULL,
	Alt bit NOT NULL,
	Shift bit NOT NULL,
	Key varchar(1) NOT NULL
);

CREATE TABLE Task ( 
	TaskID bigint NOT NULL,
	TaskName varchar(50) NOT NULL,
	Description text,
	Active bit NOT NULL
);

CREATE TABLE TaskTime ( 
	TimeId bigint NOT NULL,
	TaskId bigint NOT NULL,
	StartTime datetime,
	StopTime datetime
);


ALTER TABLE Task
	ADD CONSTRAINT UQ_Task_TaskName UNIQUE (TaskName);

ALTER TABLE Shortcut ADD CONSTRAINT PK_Shortcut 
	PRIMARY KEY CLUSTERED (ShortcutId);

ALTER TABLE Task ADD CONSTRAINT PK_Task 
	PRIMARY KEY CLUSTERED (TaskID);

ALTER TABLE TaskTime ADD CONSTRAINT PK_Time 
	PRIMARY KEY CLUSTERED (TimeId);



ALTER TABLE Shortcut ADD CONSTRAINT FK_Shortcut_Task 
	FOREIGN KEY (TaskId) REFERENCES Task (TaskID);

ALTER TABLE TaskTime ADD CONSTRAINT FK_Time_Task 
	FOREIGN KEY (TaskId) REFERENCES Task (TaskID);
