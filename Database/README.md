# SQL Test Assignment

Attached is a mysqldump of a database to be used during the test.

Below are the questions for this test. Please enter a full, complete, working SQL statement under each question. We do not want the answer to the question. We want the SQL command to derive the answer. We will copy/paste these commands to test the validity of the answer.

**Example:**

_Q. Select all users_

- Please return at least first_name and last_name

SELECT first_name, last_name FROM users;


------

**— Test Starts Here —**

1. Select users whose id is either 3,2 or 4
- Please return at least: all user fields
Answer - 
select * from users where id in (2,3,4)

2. Count how many basic and premium listings each active user has
- Please return at least: first_name, last_name, basic, premium
Answer - 
select u.id,u.first_name,u.last_name,tempResult.basic,tempResult.premium from (
select tempBasic.id,tempBasic.basic,tempPremium.premium from (
select u.id,count(tempListings.id) as basic from users u left join(
select * from listings where status = 2
 ) as tempListings on u.id = tempListings.user_id
 group by u.id
) as tempBasic
full join (
select * from (
select u.id,count(tempListings.id) as premium from users u left join(
select * from listings where status = 3
 ) as tempListings on u.id = tempListings.user_id
 group by u.id
)as tempPremium 
) as tempPremium
on tempBasic.id = tempPremium.id
) as tempResult join users u on tempResult.id = u.id where u.status = 2
order by u.id

3. Show the same count as before but only if they have at least ONE premium listing
- Please return at least: first_name, last_name, basic, premium
Answer - 
select u.id,u.first_name,u.last_name,tempResult.basic,tempResult.premium from (
select tempBasic.id,tempBasic.basic,tempPremium.premium from (
select u.id,count(tempListings.id) as basic from users u left join(
select * from listings where status = 2
 ) as tempListings on u.id = tempListings.user_id
 group by u.id
) as tempBasic
full join (
select * from (
select u.id,count(tempListings.id) as premium from users u left join(
select * from listings where status = 3
 ) as tempListings on u.id = tempListings.user_id
 group by u.id
)as tempPremium 
) as tempPremium
on tempBasic.id = tempPremium.id
) as tempResult join users u on tempResult.id = u.id where u.status = 2 and tempResult.premium>=1
order by u.id 

4. How much revenue has each active vendor made in 2013
- Please return at least: first_name, last_name, currency, revenue
Answer - 
select u.id, u.first_name, u.last_name, tempYear.currency, tempYear.revenue, tempYear.years from users u left join (
 select * from(
select u.id,u.first_name,u.last_name,c.currency,sum(c.price) as revenue, extract(year from c.created) as years from users u join listings l on u.id = l.user_id join clicks c on l.id = c.listing_id where u.status = 2 group by (u.id,u.first_name,u.last_name,c.currency,c.created)
) as temp where years = 2013
) as tempYear on u.id = tempYear.id where u.status = 2

5. Insert a new click for listing id 3, at $4.00
- Find out the id of this new click. Please return at least: id
Answer - 
INSERT INTO public.clicks(
            listing_id, price, currency, created)
    VALUES (3, 4, 'USD', now());
Id - 33

6. Show listings that have not received a click in 2013
- Please return at least: listing_name
Answer - 
select name from listings except (
select name from(
select l.name,extract(year from c.created) as year from listings l left join clicks c on l.id = c.listing_id group by l.id,l.name,c.created
) as temp where year = 2013
)

7. For each year show number of listings clicked and number of vendors who owned these listings
- Please return at least: date, total_listings_clicked, total_vendors_affected


8. Return a comma separated string of listing names for all active vendors
- Please return at least: first_name, last_name, listing_names
Answer - 
select u.id,u.first_name,u.last_name,string_agg(l.name::text, ',') as listing_names from users u join 
listings l on u.id = l.user_id
where u.status = 2
group by u.id,u.first_name,u.last_name