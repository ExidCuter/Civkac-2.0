const express = require('express');
const path = require('path');
const request = require('ajax-request');
const bodyParser = require('body-parser');
const methodOverride = require('method-override');
const session = require('express-session');

const api = require("./API/api.js");
const validator = require("./logic/simple-validator.js");
const debuher = require("./logic/debuher.js");

const polda = express();


polda.set("views", path.join(__dirname, "views"));
polda.set("view engine", "pug");

polda.use(bodyParser.urlencoded({ extended: false }));
polda.use(bodyParser.json());

polda.use(methodOverride());

polda.use(express.static(path.join(__dirname, "public")));

polda.use(session({
  secret: 'papa bless',
  resave: false,
  saveUninitialized: true
}));

polda.listen(80, () => {
  debuher.debuh("Polda: Zgem kodo na portu 80");
});

polda.get("/", (req, res) => {
  if (!req.session.user) {
    res.render("index");
  }
  else {
    res.redirect("/home");
  }
});

polda.get("/home", (req, res) => {
  if (!req.session.user) {
    res.redirect("/");
  }
  else {
    api.renderFeed(req, res, req.session.user.username);
  }
});

polda.get("/user/:username", (req, res) => {
  api.renderUser(req, res, req.params.username);
});

polda.get("/post/:id", (req, res) => {
  api.renderPost(req, res, req.params.id);
});

polda.get("/reply/:id", (req, res) => {
  api.renderReply(req, res, req.params.id);
});

polda.get("/testError", (req, res) => {
  res.status(500).send({ error: 'Something failed!' })
});

polda.post("/user/register", (req, res) => {
  let validated = validator.validateRegister(req.body);
  if (validated.failed == null) {
    api.registerUser(req, res, validated);
  }
  res.redirect("/");
});

polda.post("/user/login", (req, res) => {
  let user = {
    username: req.body.username,
    password: req.body.password
  };
  api.loginUser(req, res, user);
});

polda.get("/logout", (req, res) => {
  if (!req.session.user) {
    res.redirect("/");
  }
  else {
    req.session.user = null;
    res.redirect("/");
  }
});

polda.get("/about", (req, res) => {
    res.render("about", {
      loggedUser: req.session.user
    });
});

polda.post("/tweet", (req, res) => {
  if (!req.session.user) {
    res.redirect("/");
  }
  else {
    let data = {
      username: req.session.user.username,
      password: req.session.user.password,
      text: req.body.text
    }
    api.tweet(req, res, data);
  }

});

polda.post("/reply/:id", (req, res) => {
  if (!req.session.user) {
    res.redirect("/");
  }
  else {
    let data = {
      id: req.params.id,
      username: req.session.user.username,
      password: req.session.user.password,
      text: req.body.text
    }
    api.reply(req, res, data);
  }
});

polda.post("/follow/:user2", (req, res) => {
  if (!req.session.user) {
    res.redirect("/");
  } else {
    let data = {
      username1: req.session.user.username,
      username2: req.params.user2,
      password: req.session.user.password
    };
    api.followOrUnfollow(req, res, data);
  }
});

polda.post("/findUser", (req, res) => {
  api.renderUsers(req,res, req.body.userSearch);
});
