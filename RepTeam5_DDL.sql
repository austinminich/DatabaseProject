DROP TABLE IF EXISTS Business;
CREATE TABLE Business (
	business_id		CHAR(22),
	full_address	VARCHAR(100),
	city			VARCHAR(40),
	state			CHAR(2),
	zipcode			VARCHAR(10),
	open		    BOOLEAN,
	review_count	INTEGER DEFAULT 0,
	name			VARCHAR(100),
	longitude		FLOAT,
	latitude		FLOAT,
	stars			FLOAT DEFAULT 0,
    num_checkins    INTEGER DEFAULT 0,
	PRIMARY KEY		(business_id)
);

DROP TABLE IF EXISTS YelpUser;
CREATE TABLE YelpUser (
    user_id         CHAR(22),
    name            VARCHAR(50),
    password        VARCHAR(20),
    review_count    INTEGER DEFAULT 0,
    fans            INTEGER DEFAULT 0,
    average_stars   FLOAT DEFAULT 0,
    yelping_since   DATE NOT NULL,
    PRIMARY KEY (user_id)
);

DROP TABLE IF EXISTS Checkin;
--From business
CREATE TABLE Checkin (
    hour_day VARCHAR (5) NOT NULL,
    amount INTEGER DEFAULT 0,
    business_id CHAR(22) NOT NULL,
    PRIMARY KEY (hour_day, business_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

DROP TABLE IF EXISTS Review;
CREATE TABLE Review (
    review_id SERIAL,
    text TEXT,
    likes INTEGER,
    date DATE,
    user_id CHAR(22),
    business_id CHAR(22),
    PRIMARY KEY (review_ID),
    FOREIGN KEY (user_id) REFERENCES YelpUser(user_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

DROP TABLE IF EXISTS Votes;
--This is created within User
CREATE TABLE Votes (
    cool INTEGER,
    funny INTEGER,
    useful INTEGER,
    user_id CHAR(22),
    PRIMARY KEY (user_id),
    FOREIGN KEY (user_id) REFERENCES YelpUser(user_id)
);

DROP TABLE IF EXISTS Hours;
--From business
CREATE TABLE Hours (
    day VARCHAR (9),
    close CHAR (5),
    open CHAR (5),
    business_id CHAR(22),
    PRIMARY KEY (day, business_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

DROP TABLE IF EXISTS Categories;
--From business
CREATE TABLE Categories (
    text VARCHAR (40),
    business_id CHAR(22),
    PRIMARY KEY (text, business_id),
    FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

DROP TABLE IF EXISTS Compliment;
--From user
CREATE TABLE Compliment (
    user_id         CHAR(22),
    number          INTEGER,
    text            VARCHAR(20),
    FOREIGN KEY (user_id) REFERENCES YelpUser(user_id),
    PRIMARY KEY (text, user_id)
);

DROP TABLE IF EXISTS Friends;
-- from user
CREATE TABLE Friends (
    relation_id     SERIAL,
    user_id         CHAR(22),
    friend_id       CHAR(22),
    FOREIGN KEY (user_id) REFERENCES YelpUser(user_id),
    FOREIGN KEY (friend_id) REFERENCES YelpUser(user_id),
    PRIMARY KEY (relation_id)
);

/*
--From user
CREATE TABLE friend (
    user_id         CHAR(22),
    friend_with_id  CHAR(22),
    FOREIGN KEY (user_id, friend_with_id) REFERENCES YelpUser(user_id,user_id),
    PRIMARY KEY (user_id, friend_with_id)
);
*/