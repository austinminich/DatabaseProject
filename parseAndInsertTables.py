import json
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            zipcode = data['full_address'].split(" ")
            zipcode = zipcode[len(zipcode) - 1]
            print(str(zipcode))
            sql_str = "INSERT INTO Business (business_id, name, full_address,state,city,latitude,longitude,stars,review_count,num_checkins,open,zipcode) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["full_address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + "," + str(data["stars"]) + "," + str(data["review_count"]) + ",0 ,"  + \
                      int2BoolStr(data["open"]) + ",'" + str(zipcode) + "');"
            try:
                cur.execute(sql_str)
                print("inserted line {}", count_line)
            except:
                print("Insert to businessTABLE failed! " + str(count_line))
                #print(errorcodes.lookup(e.pgcode[:2]))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def insert2YelpUserTable():
    #reading the JSON file
    with open('yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            sql_str = "INSERT INTO YelpUser (user_id, name, review_count, fans, average_stars, yelping_since) " \
                      "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + str(data["review_count"]) + "','" + \
                      str(data["fans"]) + "','" + str(data["average_stars"]) + "','" + str(data["yelping_since"]) + "-01');"
            sql_str += "INSERT INTO Votes (cool, funny, useful, user_id)" \
                        "VALUES ('" + str(data['votes']['cool']) + "','" + str(data['votes']['funny']) + "','" + str(data['votes']['useful']) + \
                        "','" + cleanStr4SQL(data['user_id']) + "');"
            try:
                cur.execute(sql_str)
                print("inserted line {}", count_line)
            except:
                print("Insert to YelpUser failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def insert2CheckinTable():
    #reading the JSON file
    with open('yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            for key, value in data['checkin_info'].items():
                sql_str = "INSERT INTO Checkin (business_id, hour_day, amount) " \
                          "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + str(key) + "','" + str(value) + "');"
                try:
                    cur.execute(sql_str)
                    print("inserted line {}", count_line)
                except:
                    print("Insert to CheckinTABLE failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def insert2ReviewTable():
    #reading the JSON file
    with open('yelp_tip.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            '''sql_str = "INSERT INTO Review (review_id, text, likes, date, user_id, business_id) " \
                      "VALUES (" + str(count_line + 1) + ",'" + cleanStr4SQL(data['text']) + "','" + str(data['likes']) + "','" + \
                      cleanStr4SQL(data['date']) + "','" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data['business_id']) + "');"'''
            sql_str = "INSERT INTO Review (text, likes, date, user_id, business_id) " \
                      "VALUES ('" + cleanStr4SQL(data['text']) + "','" + str(data['likes']) + "','" + \
                      cleanStr4SQL(data['date']) + "','" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data['business_id']) + "');"
            try:
                cur.execute(sql_str)
                print("inserted line {}", count_line)
            except Exception as e:
                print("Insert to HoursTABLE failed! " + str(count_line))
                #print(errorcodes.lookup(e.pgcode[:2]))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def insert2HoursTable():
    #reading the JSON file
    with open('yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            for key, value in data['hours'].items():
                sql_str = "INSERT INTO Hours (business_id, day, close, open) " \
                          "VALUES ('" + str(data["business_id"]) + "','" + cleanStr4SQL(key) + "','" + cleanStr4SQL(value['close']) + "','" + \
                          cleanStr4SQL(value['open']) + "');"
                try:
                    cur.execute(sql_str)
                    print("inserted line {}", count_line)
                except:
                    print("Insert to HoursTABLE failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    

def insert2CategoriesTable():
    #reading the JSON file
    with open('yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes]
            for category in data['categories']:
                sql_str = "INSERT INTO Categories (business_id, text) " \
                          "VALUES ('" + str(data["business_id"]) + "','" + cleanStr4SQL(category) + "');"
                try:
                    cur.execute(sql_str)
                    print("inserted line {}", count_line)
                except:
                    print("Insert to CategoriesTABLE failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2ComplimentsTable():
    #reading the JSON file
    with open('yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes]
            for key, value in data['compliments'].items():
                sql_str = "INSERT INTO Compliment (user_id, text, number) " \
                          "VALUES ('" + str(data["user_id"]) + "','" + str(key) + "'," + str(value) + ");"
                try:
                    cur.execute(sql_str)
                    print("inserted line {}", count_line)
                except:
                    print("Insert to ComplimentsTABLE failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def insert2FriendsTable():
    #reading the JSON file
    with open('yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes]
            user_id = data['user_id']
            for friend in data['friends']:
                sql_str = "INSERT INTO Friends (user_id, friend_id) " \
                          "VALUES ('" + str(user_id) + "','" + str(friend) + "');"
                try:
                    cur.execute(sql_str)
                    print("inserted line {}", count_line)
                except Exception as e:
                    print("Insert to FreindsTABLE failed! " + str(count_line))
                    print(errorcodes.lookup(e.pgcode[:2]))
            
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    
def updateBusinessZipcode():
    #reading the JSON file
    with open('yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: Assuming the database name and account name is kept default, only need to change password
        try:
            conn = psycopg2.connect("dbname='postgres' user='postgres' host='localhost' password='123'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            zipcode = data['full_address'].split(" ")
            zipcode = zipcode[len(zipcode) - 1]
            sql_str = "UPDATE Business SET zipcode = '" + str(zipcode) + \
            "' WHERE business_id = '" + data['business_id'] + "'"
            try:
                cur.execute(sql_str)
            except:
                print("Update BusinessTABLE failed! " + str(count_line))
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()
    

    
###### Uncomment the lines of code below to populate the database. ######

#insert2BusinessTable()
#insert2YelpUserTable()
#insert2CheckinTable()
insert2ReviewTable()
#insert2HoursTable()
#insert2CategoriesTable()
insert2ComplimentsTable()
#insert2FriendsTable()

#updateBusinessZipcode()
