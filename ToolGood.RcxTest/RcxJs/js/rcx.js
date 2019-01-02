

function rcx(data, pass) {
    function GetKey(pass, klen) {
        var mBox = [];
        for (var i = 0; i < klen; i++) {
            mBox.push(i);
        }
        var j = 0;
        for (var i = 0; i < klen; i++) {
            var p = pass.charCodeAt(i % pass.length)

            j = (j + mBox[i] + p) % klen;
            var temp = mBox[i];
            mBox[i] = mBox[j];
            mBox[j] = temp;
        }
        return mBox;
    }
    var mBox = GetKey(pass, 256);
    var bytes = data;
    if (typeof data === 'string') {
        bytes = stringToByte(data);
    }
    var output = [];
    var i = 0, j = 0;
    for (var offset = 0; offset < bytes.length; offset++) {
        i = (++i) & 0xFF;
        j = (j + mBox[i]) & 0xFF;

        var a = bytes[offset];
        var c = (a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
        output.push(c);

        var temp2 = mBox[c];
        mBox[c] = mBox[a];
        mBox[a] = temp2;
        j = (j + a + c);
    }
    return output;
}
function threeRcx(data, pass) {
    var output = rcx(data, pass);
    output.reverse();
    output = rcx(output, pass);
    output.reverse();
    output = rcx(output, pass);
    return output;
}

function stringToByte(str) {
    var bytes = new Array();
    var len, c;
    len = str.length;
    for (var i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if (c >= 0x010000 && c <= 0x10FFFF) {
            bytes.push(((c >> 18) & 0x07) | 0xF0);
            bytes.push(((c >> 12) & 0x3F) | 0x80);
            bytes.push(((c >> 6) & 0x3F) | 0x80);
            bytes.push((c & 0x3F) | 0x80);
        } else if (c >= 0x000800 && c <= 0x00FFFF) {
            bytes.push(((c >> 12) & 0x0F) | 0xE0);
            bytes.push(((c >> 6) & 0x3F) | 0x80);
            bytes.push((c & 0x3F) | 0x80);
        } else if (c >= 0x000080 && c <= 0x0007FF) {
            bytes.push(((c >> 6) & 0x1F) | 0xC0);
            bytes.push((c & 0x3F) | 0x80);
        } else {
            bytes.push(c & 0xFF);
        }
    }
    return bytes;
}

function byteToString(arr) {
    if (typeof arr === 'string') {
        return arr;
    }
    var str = '',
        _arr = arr;
    for (var i = 0; i < _arr.length; i++) {
        var one = _arr[i].toString(2),
            v = one.match(/^1+?(?=0)/);
        if (v && one.length == 8) {
            var bytesLength = v[0].length;
            var store = _arr[i].toString(2).slice(7 - bytesLength);
            for (var st = 1; st < bytesLength; st++) {
                store += _arr[st + i].toString(2).slice(2);
            }
            str += String.fromCharCode(parseInt(store, 2));
            i += bytesLength - 1;
        } else {
            str += String.fromCharCode(_arr[i]);
        }
    }
    return str;
}

function arrayToHex(data) {
    var val = "";
    for (var i = 0; i < data.length; i++) {
        val += data[i].toString(16);
    }
    return val;
}
function hexToArray(str) {
    var data = [];
    for (var i = 0; i < str.length; i += 2) {
        var t = str[i] + str[i + 1];
        data.push(parseInt(t, 16));
    }
    return data;
}

var b64map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var b64padchar = "=";

function hex2b64(h) {
    var i;
    var c;
    var ret = "";
    for (i = 0; i + 3 <= h.length; i += 3) {
        c = parseInt(h.substring(i, i + 3), 16);
        ret += b64map.charAt(c >> 6) + b64map.charAt(c & 63);
    }
    if (i + 1 == h.length) {
        c = parseInt(h.substring(i, i + 1), 16);
        ret += b64map.charAt(c << 2);
    }
    else if (i + 2 == h.length) {
        c = parseInt(h.substring(i, i + 2), 16);
        ret += b64map.charAt(c >> 2) + b64map.charAt((c & 3) << 4);
    }
    while ((ret.length & 3) > 0) ret += b64padchar;
    return ret;
}
function b64tohex(s) {
    var ret = ""
    var i;
    var k = 0; // b64 state, 0-3
    var slop;
    for (i = 0; i < s.length; ++i) {
        if (s.charAt(i) == b64padchar) break;
        v = b64map.indexOf(s.charAt(i));
        if (v < 0) continue;
        if (k == 0) {
            ret += int2char(v >> 2);
            slop = v & 3;
            k = 1;
        }
        else if (k == 1) {
            ret += int2char((slop << 2) | (v >> 4));
            slop = v & 0xf;
            k = 2;
        }
        else if (k == 2) {
            ret += int2char(slop);
            ret += int2char(v >> 2);
            slop = v & 3;
            k = 3;
        }
        else {
            ret += int2char((slop << 2) | (v >> 4));
            ret += int2char(v & 0xf);
            k = 0;
        }
    }
    if (k == 1)
        ret += int2char(slop << 2);
    return ret;
}
function int2char(num) {
  return  String.fromCharCode(num);
}
function arrayToBase64(data) {
    return hex2b64(arrayToHex(data))
}
function base64ToArray(str) {
    return hexToArray(b64tohex(str))
}