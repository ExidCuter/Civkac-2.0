module.exports.validateRegister = function(object){
  if (object.username !== "" && object.handle !== "" && object.email !== ""
   && object.password1 !== "" && object.password2 !== "") {
     if (object.username.length >= 5 && object.handle.length >= 5 && object.email.length > 5
      && object.password1.length >= 8 && object.password2.length >= 8) {
        if (object.password1 === object.password2) {
            return {
              username: object.username,
              handle: object.handle,
              email: object.email,
              password: object.password1
            };
        }
     }
  }
  return {failed: true};
};
