-- 🔹 Users table
CREATE TABLE IF NOT EXISTS Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    FirebaseUID VARCHAR(255) UNIQUE NOT NULL,
    Name VARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 🔹 Exercises table
CREATE TABLE IF NOT EXISTS Exercises (
    ExerciseID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT NOT NULL,
    Score INT DEFAULT 0,
    DurationMinutes FLOAT DEFAULT 0,
    Successes INT DEFAULT 0,
    Failures INT DEFAULT 0,
    ExerciseDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- 🔹 Sessions table
CREATE TABLE IF NOT EXISTS Sessions (
    SessionID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT NOT NULL,
    FirebaseUID VARCHAR(255) NOT NULL,
    SessionUid VARCHAR(255) NOT NULL,
    StartTime DATETIME,
    EndTime DATETIME,
    DurationMinutes FLOAT DEFAULT 0,
    Score INT DEFAULT 0,
    Errors INT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- 🔹 Scores table
CREATE TABLE IF NOT EXISTS Scores (
    ScoreID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT NOT NULL,
    SessionUid VARCHAR(255) NOT NULL,
    ExerciseID INT DEFAULT 0,
    Score INT DEFAULT 0,
    Errors INT DEFAULT 0,
    TimeSpent FLOAT DEFAULT 0,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
    -- Pas de FK sur ExerciseID si tu veux poster des scores génériques
);
