CREATE FUNCTION UpdateReviewCount() RETURNS TRIGGER AS $$
    BEGIN
        UPDATE Business
        SET review_count = CountTable.num
        FROM
            (SELECT BUSINESS.business_id, COUNT(*) as num
            FROM BUSINESS, REVIEW
            WHERE REVIEW.business_id = Business.business_id
            GROUP BY (BUSINESS.business_id)) AS CountTable
        WHERE Business.business_id = CountTable.business_id;
        
        UPDATE YelpUser
        SET review_count = CountTable.num
        FROM
            (SELECT YelpUser.user_id, COUNT(*) as num
            FROM YelpUser, REVIEW
            WHERE REVIEW.user_id = YelpUser.user_id
            GROUP BY (YelpUser.user_id)) AS CountTable
        WHERE YelpUser.user_id = CountTable.user_id;
        RETURN NULL;
    END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER ReviewInserted
    AFTER INSERT OR UPDATE ON Review
    EXECUTE PROCEDURE UpdateReviewCount();
    
CREATE FUNCTION UpdateCheckinCount() RETURNS TRIGGER AS $$
    BEGIN
        UPDATE Business
        SET num_checkins = CountTable.num
        FROM
            (SELECT BUSINESS.business_id, COUNT(*) as num
            FROM BUSINESS, CHECKIN
            WHERE CHECKIN.business_id = Business.business_id
            GROUP BY (BUSINESS.business_id)) AS CountTable
        WHERE Business.business_id = CountTable.business_id;
        RETURN NULL;
    END;
$$ LANGUAGE plpgsql;
    
CREATE TRIGGER CheckinInserted
    AFTER INSERT OR UPDATE ON Checkin
    EXECUTE PROCEDURE UpdateCheckinCount();
    
------------------------------------------------------------------
--The trigger is being called but the tuple is still being inserted.
--Attempted to fix the problem for a while but couldn't figure it out.
--Feedback would be appreciated
------------------------------------------------------------------
    
CREATE FUNCTION CheckIfClosed() RETURNS TRIGGER AS $$
    BEGIN
        SELECT open AS o
        FROM Business
        WHERE NEW.business_id = Business.business_id;
        
        IF (o = true) THEN
            INSERT INTO Checkin(hour_day, amount, business_id)
            VALUES (NEW.hour_day, NEW.amount, NEW.business_id);
        END IF;
        RETURN NULL;
    END;
$$ LANGUAGE plpgsql;
    
CREATE TRIGGER CheckinInsertCheck
    BEFORE INSERT ON Checkin
    EXECUTE PROCEDURE CheckIfClosed();
    
------------------------------------------------------------------