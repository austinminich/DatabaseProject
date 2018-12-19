--Business Indexes
CREATE INDEX "businessIndex"
    ON public.business USING hash
    (business_id)
    TABLESPACE pg_default;
CREATE INDEX "businessIndex_state"
    ON public.business USING hash
    (state)
    TABLESPACE pg_default;
CREATE INDEX "businessIndex_city"
    ON public.business USING hash
    (city)
    TABLESPACE pg_default;
CREATE INDEX "businessIndex_zipcode"
    ON public.business USING hash
    (zipcode)
    TABLESPACE pg_default;
    
--Categories INDEXES
CREATE INDEX "categoriesIndex_business_id"
    ON public.categories USING hash
    (business_id)
    TABLESPACE pg_default;
CREATE INDEX "categoriesIndex_category"
    ON public.categories USING hash
    (text)
    TABLESPACE pg_default;

--Checkin INDEXES
CREATE INDEX "checkinIndex_business_id"
    ON public.checkin USING hash
    (business_id)
    TABLESPACE pg_default;
    
--Compliment INDEXES
--CREATE INDEX complimentTable_user_id ON Compliment(user_id);

--Friends INDEXES
CREATE INDEX "friendsIndex_user_id"
    ON public.friends USING hash
    (user_id)
    TABLESPACE pg_default;
CREATE INDEX friends_friend_id
    ON public.friends USING hash
    (friend_id)
    TABLESPACE pg_default;
    
--Hours INDEXES
CREATE INDEX "hoursIndex_day"
    ON public.hours USING hash
    (day)
    TABLESPACE pg_default;
CREATE INDEX "hoursIndex_business_id"
    ON public.hours USING hash
    (business_id)
    TABLESPACE pg_default;
CREATE INDEX "hoursIndex_open"--btree
    ON public.hours USING btree
    (open ASC NULLS LAST)
    TABLESPACE pg_default;
CREATE INDEX "hoursIndex_close"--btree
    ON public.hours USING btree
    (close ASC NULLS LAST)
    TABLESPACE pg_default;
    
--Review INDEXES
CREATE INDEX "reviewIndex_business_id"
    ON public.review USING hash
    (business_id)
    TABLESPACE pg_default;
CREATE INDEX "reviewIndex_user_id"
    ON public.review USING hash
    (user_id)
    TABLESPACE pg_default;
    
--Votes INDEXES
CREATE INDEX "votesIndex_user_id"
    ON public.votes USING hash
    (user_id)
    TABLESPACE pg_default;
    
--YelpUser INDEXES
CREATE INDEX "yelpUser_user_id"
    ON public.yelpuser USING hash
    (user_id)
    TABLESPACE pg_default;
CREATE INDEX "yelpUser_name"
    ON public.yelpuser USING hash
    (name)
    TABLESPACE pg_default;
