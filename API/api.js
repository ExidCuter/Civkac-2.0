const request = require('ajax-request');

module.exports.renderUsers = function(req, res, like){
  request({
    url: 'http://localhost:6331/api/user/like/' + like,
    method: 'GET',
    json: true
  }, function(err, resp, body) {
    res.render("users", {
      users: body,
      loggedUser: req.session.user
    });
  });
};


module.exports.renderFeed = function(req, res, username){
  request({
    url: 'http://localhost:6331/api/feed/user/' + username,
    method: 'GET',
    json: true
  }, function(err, resp, body) {
    res.render("home", {
      posts: body,
      loggedUser: req.session.user
    });
  });
};

module.exports.renderPost = function(req, res, id){
  request({
    url: 'http://localhost:6331/api/post/' + id,
    method: 'GET',
    json: true
  }, function(err, resp, body) {
    res.render("post", {
      post: body,
      loggedUser: req.session.user
    });
  });
};

module.exports.renderReply = function(req, res, id){
  request({
    url: 'http://localhost:6331/api/reply/get/' + id,
    method: 'GET',
    json: true
  }, function(err, resp, body) {
    res.render("reply", {
      reply: body,
      loggedUser: req.session.user
    });
  });
};

module.exports.renderUser = function(req, res, username){
  let url = 'http://localhost:6331/api/user/username/' + username;
  if (req.session.user) {
    url = 'http://localhost:6331/api/user/username/' + username + '?logged=' + req.session.user.username;
  }
  request({
    url: url,
    method: 'GET',
    json: true
  }, function(err, resp, body) {
    if (typeof body === 'string' || body instanceof String)
    {
      res.send("Ups Polda ni nasel uporabnika po imenu: " + username);
    } else {
      res.render("user", {
        user: body,
        loggedUser: req.session.user
      });
    }

  });
};


module.exports.registerUser = function(req, res, user){
  request({
    url: 'http://localhost:6331/api/user',
    method: 'POST',
    data: user,
    json: true
  }, function(err, resp, body) {
    if (err) {
      console.log(err);
    }
    else {
      if (body !== "Prazen Objekt" && body !== "Napacni podatki") {
      }
    }
  });
};

module.exports.loginUser = function(req, res, user) {
  console.log(user);
  request({
    url: 'http://localhost:6331/api/user/login',
    method: 'POST',
    data: user,
    json: true
  }, function(err, resp, body) {
    if (err) {
      console.log(err);
      console.log(user);
      res.send("nope.jpg");
    }
    else {
      if(body !== "Napacni podatki") {
        req.session.user = user;
        res.redirect("/home");
      }
      else {
        res.send("nope.jpg");
      }
    }
  });
}

module.exports.tweet = function(req, res, data) {
  request({
    url: 'http://localhost:6331/api/post/tweet',
    method: 'POST',
    data: data,
    json: true
  }, function(err, resp, body) {
    if (err) {
      console.log(body);
      res.send("nope.jpg");
    }
    else {
      if(body !== "Napacni podatki") {
        res.redirect("/home");
      }
      else {
        res.send("nope.jpg");
      }
    }
  });
}

module.exports.reply = function(req, res, data) {
  request({
    url: 'http://localhost:6331/api/reply/' + data.id,
    method: 'POST',
    data: data,
    json: true
  }, function(err, resp, body) {
    if (err) {
      console.log(body);
      res.send("nope.jpg");
    }
    else {
      if(body !== "Napacni podatki") {
        res.redirect("/home");
      }
      else {
        res.send("nope.jpg");
      }
    }
  });
}

module.exports.followOrUnfollow = function(req, res, data) {
  request({
    url: 'http://localhost:6331/api/follow/',
    method: 'POST',
    data: data,
    json: true
  }, function(err, resp, body) {
    if (err) {
      console.log(body);
      res.send("nope.jpg");
    }
    else {
      if(body !== "Napacni podatki") {
        res.redirect("/user/"+data.username2+"?logged="+data.username1);
      }
      else {
        res.send("nope.jpg");
      }
    }
  });
}
