-- 
-- 表的结构 `es_user`
-- 

--DROP TABLE IF EXISTS "es_user";
CREATE TABLE "es_user" (
  "user_id" int IDENTITY (1,1) NOT NULL,
  "user_num" varchar(100) NOT NULL,
  "user_name" varchar(100) NOT NULL,
  "user_password" varchar(100) NOT NULL,
  "user_func" smallint NOT NULL,
  "user_login" bit NOT NULL,
  "user_do" bit NOT NULL,
  PRIMARY KEY  ("user_id")
);

-- --------------------------------------------------------
-- 
-- 表的结构 `es_history`
-- 

--DROP TABLE IF EXISTS "es_history";
CREATE TABLE "es_history" (
  "history_id" int IDENTITY (1,1) NOT NULL,
  "history_stime" datetime NOT NULL,
  "history_etime" datetime NULL,
  "history_score" smallint NULL,
  "history_user" int NOT NULL,
  "history_ip" varchar(50) NULL,
  PRIMARY KEY  ("history_id")
);

-- --------------------------------------------------------

-- 
-- 表的结构 `es_link`
-- 

--DROP TABLE IF EXISTS "es_link";
CREATE TABLE "es_link" (
  "link_id" int IDENTITY (1,1) NOT NULL,
  "link_type" smallint NOT NULL,
  "link_data" datetime NOT NULL,
  "link_short" varchar(200) NOT NULL,
  "link_text" text NOT NULL,
  "link_diff" smallint NOT NULL,
  "link_bak" varchar(50) NULL,
  PRIMARY KEY  ("link_id")
);

-- --------------------------------------------------------

-- 
-- 表的结构 `es_question`
-- 

--DROP TABLE IF EXISTS "es_question";
CREATE TABLE "es_question" (
  "ques_id" int IDENTITY (1,1) NOT NULL,
  "ques_type" smallint NOT NULL,
  "ques_link" int NOT NULL,
  "ques_short" varchar(200) NOT NULL,
  "ques_data" datetime NOT NULL,
  "ques_text" text NOT NULL,
  "ques_item" text NOT NULL,
  "ques_diff" smallint NOT NULL,
  "ques_answer" varchar(50) NOT NULL,
  "ques_score" smallint NOT NULL,
  PRIMARY KEY  ("ques_id")
);


-- --------------------------------------------------------

-- 
-- 表的结构 `es_default`
-- 

--DROP TABLE IF EXISTS "es_default";
CREATE TABLE "es_default" (
  "default_type" smallint NOT NULL,
  "default_score" smallint NOT NULL
);


-- --------------------------------------------------------

--
--初始化登陆用户
--

insert into es_user(user_num,user_name,user_password,user_func,user_login,user_do) values('-123456','admin','7a57a5a743894a0e',3,0,1);