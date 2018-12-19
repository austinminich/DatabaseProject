--Updates review_count in Business with appropriate amount of reviews in database
UPDATE Business
SET review_count = CountTable.num
FROM
    (SELECT BUSINESS.business_id, COUNT(*) as num
    FROM BUSINESS, REVIEW
    WHERE REVIEW.business_id = Business.business_id
    GROUP BY (BUSINESS.business_id)) AS CountTable
WHERE Business.business_id = CountTable.business_id;

--Updates review_count in YelpUser with appropriate amount of reviews in database
UPDATE YelpUser
SET review_count = CountTable.num
FROM
    (SELECT YelpUser.user_id, COUNT(*) as num
    FROM YelpUser, REVIEW
    WHERE REVIEW.business_id = YelpUser.user_id
    GROUP BY (YelpUser.user_id)) AS CountTable
WHERE YelpUser.user_id = CountTable.user_id;

--Updates num_checkins with appropriate amount of checkins in database
UPDATE Business
SET num_checkins = CountTable.num
FROM
    (SELECT BUSINESS.business_id, SUM(amount) as num
    FROM BUSINESS, CHECKIN
    WHERE CHECKIN.business_id = Business.business_id
    GROUP BY (BUSINESS.business_id)) AS CountTable
WHERE Business.business_id = CountTable.business_id;
