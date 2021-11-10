CREATE DATABASE IF NOT EXISTS A04;
USE A04;

DROP TABLE IF EXISTS myQuestion;
CREATE TABLE myQuestion (
  questionID int DEFAULT NULL,
 questionContent varchar(100),
  potentialAnswers varchar(100),
  correctAnswer int
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO myQuestion (questionID,questionContent,potentialAnswers,correctAnswer) VALUES 
(1,'What is the answer of 1 + 1 ?','1:1 2:2 3:3 4:4',2),
(2,'What is the answer of 1234 + 1234 ?','1:2345 2:3456 3:2468 4:3468',3),
(3,'What is the answer of 3 * 4 ?','1:5 2:12 3:15 4:16',2),
(4,'What is the answer of 12 * 12 ?','1:123 2:144 3:322 4:233',2),
(5,'What is the answer of 99 * 99 ?','1:9801 2:9900 3:8901 4:9999',1),
(6,'What is the most popular drink in StarBuck?','1:Pink Drink 2:Salted Caramel Mocha 3:Eggnog Latte 4:Caramel Macchiato',4),
(7,'Which is not the size in StarBuck?','1:Short 2:Tall 3:Venti 4:Large',4),
(8,'Who is not a NBA player?','1:Michael Jordan 2:LeBron James 3:Tim Hortons 4:Kawhi Leonard',3),
(9,'Which is not a SET course?','1:Relational Database 2:Windows and Mobile Programming 3:Java Programming 4:Conestoga 101',3),
(10,'What is the answer of 9876 * 5678 ?','1:56075928 2:96325863 3:85746952 4:7896526',1);


DROP TABLE IF EXISTS myLeaderboard;
CREATE TABLE myLeaderboard (
  userName varchar(100) DEFAULT NULL,
  userScore int
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

