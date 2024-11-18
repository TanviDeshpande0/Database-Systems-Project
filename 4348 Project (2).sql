CREATE TABLE Donor (
    Donor_id INT PRIMARY KEY,
    Blood_Type VARCHAR(3),
    DOB DATE,
    Address VARCHAR(255)
);

CREATE TABLE Transfusion (
    Date DATE,
    Donor_id INT,
    Temperature FLOAT,
    Blood_Pressure VARCHAR(10),
    WBC FLOAT,
    Hemoglobin FLOAT,
    FOREIGN KEY (Donor_id) REFERENCES Donor(Donor_id)
);

CREATE TABLE Employee (
    Employee_id INT PRIMARY KEY,
    Join_date DATE,
    Salary FLOAT
);

CREATE TABLE Phlebotomist (
    Employee_id INT PRIMARY KEY,
    License_id VARCHAR(50),
    FOREIGN KEY (Employee_id) REFERENCES Employee(Employee_id)
);

CREATE TABLE Driver (
    Employee_id INT PRIMARY KEY,
    Drivers_License VARCHAR(50),
    FOREIGN KEY (Employee_id) REFERENCES Employee(Employee_id)
);

CREATE TABLE Manager (
    Employee_id INT PRIMARY KEY,
    Location VARCHAR(255),
    FOREIGN KEY (Employee_id) REFERENCES Employee(Employee_id)
);

CREATE TABLE Truck (
    Truck_id INT PRIMARY KEY,
    InspectionDate DATE
);

INSERT INTO Donor (Donor_id, Blood_Type, DOB, Address) VALUES
(1, 'O+', '1990-05-14', '123 Oak St, Dallas, TX'),
(2, 'A-', '1985-07-22', '456 Pine Ave, Houston, TX'),
(3, 'B+', '1992-11-10', '789 Maple Rd, Austin, TX'),
(4, 'AB-', '1980-02-28', '101 Elm Dr, San Antonio, TX'),
(5, 'O-', '1995-03-05', '202 Cedar St, Fort Worth, TX'),
(6, 'A+', '1988-09-12', '303 Birch Ln, Plano, TX'),
(7, 'B-', '1993-12-30', '404 Spruce Blvd, Irving, TX'),
(8, 'AB+', '1979-04-15', '505 Ash St, Denton, TX'),
(9, 'O+', '1986-08-18', '606 Poplar Ave, Richardson, TX'),
(10, 'A-', '1991-01-20', '707 Willow Rd, Garland, TX');

INSERT INTO Transfusion (Date, Donor_id, Temperature, Blood_Pressure, WBC, Hemoglobin) VALUES
('2024-01-10', 1, 98.6, '120/80', 5.1, 13.5),
('2024-01-11', 2, 99.0, '115/75', 5.5, 14.0),
('2024-01-12', 3, 98.7, '118/78', 5.3, 13.8),
('2024-01-13', 4, 98.9, '110/70', 5.6, 14.1),
('2024-01-14', 5, 98.5, '122/80', 5.0, 13.3),
('2024-01-15', 6, 99.1, '125/82', 5.4, 13.9),
('2024-01-16', 7, 98.4, '117/76', 5.2, 13.7),
('2024-01-17', 8, 98.8, '114/74', 5.7, 14.2),
('2024-01-18', 9, 99.2, '121/79', 5.3, 13.8),
('2024-01-19', 10, 98.6, '119/77', 5.1, 13.6);

INSERT INTO Employee (Employee_id, Join_date, Salary) VALUES
(1, '2020-01-05', 55000),
(2, '2019-06-15', 60000),
(3, '2021-02-20', 52000),
(4, '2018-09-01', 58000),
(5, '2022-04-10', 50000),
(6, '2017-11-25', 62000),
(7, '2023-03-30', 48000),
(8, '2021-12-05', 53000),
(9, '2020-08-14', 57500),
(10, '2022-07-22', 50500);

INSERT INTO Phlebotomist (Employee_id, License_id) VALUES
(1, 'PHL12345'),
(2, 'PHL67890'),
(3, 'PHL23456'),
(4, 'PHL34567'),
(5, 'PHL45678'),
(6, 'PHL56789'),
(7, 'PHL67891'),
(8, 'PHL78901'),
(9, 'PHL89012'),
(10, 'PHL90123');

INSERT INTO Driver (Employee_id, Drivers_License) VALUES
(1, 'DL12345'),
(2, 'DL67890'),
(3, 'DL23456'),
(4, 'DL34567'),
(5, 'DL45678'),
(6, 'DL56789'),
(7, 'DL67891'),
(8, 'DL78901'),
(9, 'DL89012'),
(10, 'DL90123');

INSERT INTO Manager (Employee_id, Location) VALUES
(1, 'Dallas, TX'),
(2, 'Houston, TX'),
(3, 'Austin, TX'),
(4, 'San Antonio, TX'),
(5, 'Fort Worth, TX'),
(6, 'Plano, TX'),
(7, 'Irving, TX'),
(8, 'Denton, TX'),
(9, 'Richardson, TX'),
(10, 'Garland, TX');

INSERT INTO Truck (Truck_id, InspectionDate) VALUES
(1, '2023-07-15'),
(2, '2023-08-20'),
(3, '2023-09-10'),
(4, '2023-10-25'),
(5, '2023-11-05'),
(6, '2023-12-15'),
(7, '2024-01-05'),
(8, '2024-02-20'),
(9, '2024-03-15'),
(10, '2024-04-10');

