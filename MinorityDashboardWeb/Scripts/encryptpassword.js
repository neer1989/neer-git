function EncryptWithSalt(pass, salt) {
    var algo = CryptoJS.algo.SHA256.create();
    algo.update(pass, 'utf-8');
    algo.update(CryptoJS.SHA256(salt), 'utf-8');
    var hash = algo.finalize().toString(CryptoJS.enc.Base64);
    return hash;
}