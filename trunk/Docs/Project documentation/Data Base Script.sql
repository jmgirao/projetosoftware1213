SET FOREIGN_KEY_CHECKS=0;



DROP TABLE IF EXISTS Inactivity CASCADE;
DROP TABLE IF EXISTS Shortcut CASCADE;
DROP TABLE IF EXISTS Task CASCADE;
DROP TABLE IF EXISTS TaskTime CASCADE;

CREATE TABLE Inactivity
(
	InactivityID BIGINT NOT NULL,
	InactivityEnabled BOOL NOT NULL,
	InactivityTime TINYINT,
	PRIMARY KEY (InactivityID)

) ;


CREATE TABLE Shortcut
(
	ShortcutId TINYINT NOT NULL,
	TaskId BIGINT NOT NULL,
	Ctrl BOOL NOT NULL,
	Alt BOOL NOT NULL,
	Shift BOOL NOT NULL,
	Key VARCHAR(1) NOT NULL,
	PRIMARY KEY (ShortcutId),
	KEY (TaskId)

) ;


CREATE TABLE Task
(
	TaskID BIGINT NOT NULL,
	TaskName VARCHAR(50) NOT NULL,
	Description TEXT,
	Active BOOL NOT NULL,
	PRIMARY KEY (TaskID),
	UNIQUE UQ_Task_TaskName(TaskName)

) ;


CREATE TABLE TaskTime
(
	TimeId BIGINT NOT NULL,
	TaskId BIGINT NOT NULL,
	StartTime DATETIME,
	StopTime DATETIME,
	PRIMARY KEY (TimeId),
	KEY (TaskId)

) ;



SET FOREIGN_KEY_CHECKS=1;


ALTER TABLE Shortcut ADD CONSTRAINT FK_Shortcut_Task 
	FOREIGN KEY (TaskId) REFERENCES Task (TaskID);

ALTER TABLE TaskTime ADD CONSTRAINT FK_Time_Task 
	FOREIGN KEY (TaskId) REFERENCES Task (TaskID);
