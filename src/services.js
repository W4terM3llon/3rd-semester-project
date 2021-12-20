
export function setJWT(token){
    var expireInMinutes = 60 * 12
    var expiryDate = new Date(new Date().getTime() + expireInMinutes*60000);
    document.cookie = `JWT=${token}; expires=${expiryDate.toUTCString()}; path=/`
}

export function getJWT(){
    var jwt = getCookie('JWT')
    return jwt
}

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    let expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
  }

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) === ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) === 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }

export function getTimeString(dayPeriod) {
    return `${dayPeriod < 60 * 10 ? "0" : ""}${Math.floor(dayPeriod / 60)}${
    dayPeriod % 60 === 0
        ? ""
        : dayPeriod % 60 < 10
        ? `:0${dayPeriod % 60}`
        : `:${dayPeriod % 60}`
    }`;
}