CREATE KEYSPACE db WITH REPLICATION = {'class':'SimpleStrategy', 'replication_factory':1}";
use db;
DROP TABLE bus_stops;
CREATE TABLE bus_stops (
    id int PRIMARY KEY,
    title text,
    longitude double,
    latitude double
);

