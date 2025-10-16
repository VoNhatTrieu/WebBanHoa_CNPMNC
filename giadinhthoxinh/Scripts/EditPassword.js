// File: /Scripts/editPassword.js
const newPassword = document.getElementById('newPassword');
const confirmPassword = document.getElementById('confirmPassword');
const passwordForm = document.getElementById('passwordForm');

newPassword.addEventListener('input', function () {
    const value = this.value;
    const hasLength = value.length >= 8;
    const hasUpper = /[A-Z]/.test(value);
    const hasNumber = /[0-9]/.test(value);
    const hasSpecial = /[!#$%^&*(),.?":{}|<>@_\-+=\[\]\\\/~`]/.test(value);

    updateRequirement('req-length', hasLength);
    updateRequirement('req-uppercase', hasUpper);
    updateRequirement('req-number', hasNumber);
    updateRequirement('req-special', hasSpecial);
});

confirmPassword.addEventListener('input', function () {
    const matchError = document.getElementById('password-match-error');
    if (this.value && this.value !== newPassword.value) {
        matchError.style.display = 'block';
    } else {
        matchError.style.display = 'none';
    }
});

function updateRequirement(id, isValid) {
    const element = document.getElementById(id);
    if (isValid) {
        element.classList.add('valid');
    } else {
        element.classList.remove('valid');
    }
}

function togglePassword(inputId, icon) {
    const input = document.getElementById(inputId);
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}

passwordForm.addEventListener('submit', function (e) {
    const value = newPassword.value;

    if (confirmPassword.value !== value) {
        e.preventDefault();
        alert('❌ Mật khẩu xác nhận không khớp!');
        return;
    }

    const hasLength = value.length >= 8;
    const hasUpper = /[A-Z]/.test(value);
    const hasNumber = /[0-9]/.test(value);
    const hasSpecial = /[!#$%^&*(),.?":{}|<>@_\-+=\[\]\\\/~`]/.test(value);

    if (!hasLength || !hasUpper || !hasNumber || !hasSpecial) {
        e.preventDefault();
        alert('❌ Mật khẩu không đủ mạnh! Vui lòng đáp ứng tất cả các yêu cầu.');
        return;
    }

    document.getElementById('loading').classList.add('show');
});