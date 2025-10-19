-- Users table
CREATE TABLE IF NOT EXISTS Users (
    userID INT PRIMARY KEY AUTO_INCREMENT,
    firebaseUID VARCHAR(255) UNIQUE NOT NULL,
    name VARCHAR(100) NOT NULL
);

-- Exercises table
CREATE TABLE IF NOT EXISTS Exercises (
    exerciseID INT PRIMARY KEY AUTO_INCREMENT,
    userID INT NOT NULL,
    score INT,
    durationMinutes FLOAT,
    successes INT,
    failures INT,
    exerciseDate DATETIME,
    FOREIGN KEY (userID) REFERENCES Users(userID)
);

-- Scores table
CREATE TABLE IF NOT EXISTS Scores (
    scoreID INT PRIMARY KEY AUTO_INCREMENT,
    userID INT NOT NULL,
    exerciseID INT NOT NULL,
    score INT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (userID) REFERENCES Users(userID),
    FOREIGN KEY (exerciseID) REFERENCES Exercises(exerciseID)
);

-- Sessions table
CREATE TABLE IF NOT EXISTS Sessions (
    sessionID INT PRIMARY KEY AUTO_INCREMENT,
    userID INT NOT NULL,
    startTime DATETIME,
    endTime DATETIME,
    durationMinutes FLOAT,
    FOREIGN KEY (userID) REFERENCES Users(userID)
);
