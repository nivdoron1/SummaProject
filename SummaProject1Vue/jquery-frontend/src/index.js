function createUserForm(containerId, isUpdate = false, userData = {}) {
    const container = document.getElementById(containerId);
    container.innerHTML = ''; 
    const formTitle = document.createElement('h1');
    formTitle.id = 'form-title';
    formTitle.textContent = isUpdate ? 'Update User' : 'Create User';
    container.appendChild(formTitle);

    const form = document.createElement('form');
    form.id = isUpdate ? 'update-user-form' : 'user-form';

    // Create username input field
    const usernameField = document.createElement('div');
    usernameField.className = 'input-field';
    const usernameInput = document.createElement('input');
    usernameInput.id = 'username';
    usernameInput.type = 'text';
    usernameInput.className = 'validate';
    usernameInput.required = true;
    if (isUpdate) usernameInput.value = userData.username;
    const usernameLabel = document.createElement('label');
    usernameLabel.htmlFor = 'username';
    usernameLabel.textContent = 'Username';
    usernameField.appendChild(usernameInput);
    usernameField.appendChild(usernameLabel);
    form.appendChild(usernameField);
    
    if (isUpdate && userData.username) {
        usernameLabel.classList.add('active');
    }

    const emailField = document.createElement('div');
    emailField.className = 'input-field';
    const emailInput = document.createElement('input');
    emailInput.id = 'email';
    emailInput.type = 'email';
    emailInput.className = 'validate';
    emailInput.required = true;
    if (isUpdate) {
        emailInput.value = userData.email;
        emailInput.disabled = true;
    }
    const emailLabel = document.createElement('label');
    emailLabel.htmlFor = 'email';
    emailLabel.textContent = 'Email';
    const emailError = document.createElement('span');
    emailError.id = 'email-error';
    emailError.className = 'error-message';
    emailField.appendChild(emailInput);
    emailField.appendChild(emailLabel);
    emailField.appendChild(emailError);
    form.appendChild(emailField);
    
    if (isUpdate && userData.email) {
        emailLabel.classList.add('active');
    }

    // Create birthDate input field
    const birthDateField = document.createElement('div');
    birthDateField.className = 'input-field';
    const birthDateInput = document.createElement('input');
    birthDateInput.id = 'birthDate';
    birthDateInput.type = 'date';
    birthDateInput.className = 'validate';
    birthDateInput.required = true;
    if (isUpdate) birthDateInput.value = userData.birthDate;
    const birthDateLabel = document.createElement('label');
    birthDateLabel.htmlFor = 'birthDate';
    birthDateLabel.textContent = 'Date of Birth';
    const birthDateError = document.createElement('span');
    birthDateError.id = 'birthDate-error';
    birthDateError.className = 'error-message';
    birthDateField.appendChild(birthDateInput);
    birthDateField.appendChild(birthDateLabel);
    birthDateField.appendChild(birthDateError);
    form.appendChild(birthDateField);

    if (isUpdate && userData.birthDate) {
        birthDateLabel.classList.add('active');
    }

    // Set max date for birthDate input to today
    const today = new Date().toISOString().split('T')[0];
    birthDateInput.max = today;

    // Create file input field
    const fileField = document.createElement('div');
    fileField.className = 'file-field input-field';
    const fileButton = document.createElement('div');
    fileButton.className = 'btn';
    const fileButtonText = document.createElement('span');
    fileButtonText.textContent = 'Photo';
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.id = 'photo';
    fileButton.appendChild(fileButtonText);
    fileButton.appendChild(fileInput);
    fileField.appendChild(fileButton);
    const filePathWrapper = document.createElement('div');
    filePathWrapper.className = 'file-path-wrapper';
    const filePathInput = document.createElement('input');
    filePathInput.className = 'file-path validate';
    filePathInput.type = 'text';
    filePathWrapper.appendChild(filePathInput);
    fileField.appendChild(filePathWrapper);
    form.appendChild(fileField);

    if (isUpdate && userData.photo) {
        hiddenPhotoInput = userData.photo;
    }
    
    // Create submit button
    const submitButton = document.createElement('button');
    submitButton.id = 'submit-button';
    submitButton.className = 'btn waves-effect waves-light';
    submitButton.type = 'submit';
    submitButton.textContent = isUpdate ? 'Update User' : 'Create User';
    form.appendChild(submitButton);

    // Create reset button
    const resetButton = document.createElement('button');
    resetButton.id = 'reset-button';
    resetButton.className = 'btn waves-effect waves-light';
    resetButton.type = 'button';
    resetButton.textContent = 'Clear';
    form.appendChild(resetButton);

    container.appendChild(form);

    const message = document.createElement('p');
    message.id = 'message';
    container.appendChild(message);

    // Add event listener to form to validate birth date on submission
    form.addEventListener('submit', function(event) {
        const birthDateValue = new Date(birthDateInput.value);
        if (birthDateValue > new Date()) {
            birthDateError.textContent = 'Date of Birth cannot be later than today.';
            event.preventDefault();
        } else {
            birthDateError.textContent = '';
        }
    });

    // Add event listener to reset button to clear form but not email if it's an update
    resetButton.addEventListener('click', function() {
        const emailValue = emailInput.value;
        form.reset();
        if (isUpdate) {
            emailInput.value = emailValue;
            emailInput.disabled = true;
            emailLabel.classList.add('active');
        }
    });
}
createUserForm('form-container');

$(document).ready(function () {
    const API_URL = 'http://localhost:5271/';

    const createUpdateUser = (formData) => {
        return $.ajax({
            url: `${API_URL}users/createOrUpdateUser`,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                return data;
            },
            error: function (error) {
                console.error('Error creating user:', error);
                throw error;
            }
        });
    };

    const getAllUsers = () => {
        return $.ajax({
            url: `${API_URL}users/allUsers`,
            type: 'GET',
            success: function (data) {
                return data;
            },
            error: function (error) {
                console.error('Error fetching users:', error);
                throw error;
            }
        });
    };

    const getUserById = (id) => {
        return $.ajax({
            url: `${API_URL}users/${id}`,
            type: 'GET',
            success: function (data) {
                return data;
            },
            error: function (error) {
                console.error('Error fetching user details:', error);
                throw error;
            }
        });
    };

    const deleteUserById = (id) => {
        return $.ajax({
            url: `${API_URL}users/${id}`,
            type: 'DELETE',
            success: function () {
            },
            error: function (error) {
                console.error('Error deleting user by ID:', error);
                throw error;
            }
        });
    };

    const today = new Date().toISOString().split('T')[0];
    const user = null; // Set this variable to the user object if updating a user

    if (user) {
        $('#form-title').text('Update User');
        $('#username').val(user.username).focus();
        $('#email').val(user.email).attr('disabled', true);
        $('#birthDate').val(user.birthDate);
    }

    $('#user-form').on('submit', async function (event) {
        event.preventDefault();
        if (!validateForm()) {
            $('#message').text('Please fill in all required fields correctly.');
            return;
        }

        const formData = new FormData();
        formData.append('Username', $('#username').val());
        formData.append('Email', $('#email').val());
        formData.append('BirthDate', $('#birthDate').val());

        const photo = $('#photo')[0].files[0];
        if (photo) {
            formData.append('photo', photo);
        }

        try {
            const response = await createUpdateUser(formData);
            $('#message').text(`User created successfully with ID: ${response.id}`);
            handleReset();
            alert(`User created/updated successfully with ID: ${response.id}`);
            document.dispatchEvent(new Event('user-updated'));
        } catch (error) {
            $('#message').text(`Error creating/updating user: ${error.message}`);
            alert(`Error creating/updating user: ${error.message}`);
        }
    });

    $('#reset-button').on('click', handleReset);

    function validateForm() {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const email = $('#email').val();
        const birthDate = $('#birthDate').val();

        let isValid = true;

        if (email && !emailPattern.test(email)) {
            $('#email-error').text('Please enter a valid email address');
            isValid = false;
        } else {
            $('#email-error').text('');
        }

        if (birthDate && birthDate > today) {
            $('#birthDate-error').text('Birth date cannot be a future date');
            isValid = false;
        } else {
            $('#birthDate-error').text('');
        }

        return isValid;
    }

    function handleReset() {
        const emailValue = $('#email').val();
        $('#username').val('');
        $('#email').val(emailValue).attr('disabled', false);
        $('#birthDate').val('');
        $('#photo').val('');
        $('.file-path').val('');
        $('#message').text('');
        $('#submit-button');
    }

    const userList = $('#userList');
    const searchQuery = $('#searchQuery');
    const userDetailsDialog = $('#userDetailsDialog');
    const editUserDialog = $('#editUserDialog');

    let users = {}; // Replace with actual data fetching method
    let selectedUser = null;

    async function fetchUsers() {
        try {
            users = await getAllUsers(); // Replace with actual data fetching method
            renderUserList();
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    }

    function renderUserList() {
        userList.empty();
        const filteredUsers = filterUsers(users, searchQuery.val());
        $.each(filteredUsers, function (id, user) {
            userList.append(`
                <li class="collection-item" data-id="${id}">
                    ${user}
                </li>
            `);
        });
    }

    function filterUsers(users, query) {
        if (!query) return users;
        return Object.keys(users).filter(id => users[id].toLowerCase().includes(query.toLowerCase()))
            .reduce((result, id) => {
                result[id] = users[id];
                return result;
            }, {});
    }

    async function openUserDetails(userId) {
        try {
            selectedUser = await getUserById(userId); 
            $('#userDetailsUsername').text(selectedUser.username);
            $('#userDetailsEmail').text(selectedUser.email);
            $('#userDetailsBirthDate').text(selectedUser.birthDate);
    
            const imageUrl = getImageUrl(selectedUser.photo);
            if (imageUrl) {
                $('#userPhoto').attr('src', imageUrl).show();
            } else {
                $('#userPhoto').hide();
            }
    
            M.Modal.getInstance(userDetailsDialog).open();
        } catch (error) {
            console.error('Error fetching user details:', error);
        }
    }
    
    function getImageUrl(byteArray) {
        if (!byteArray || byteArray.length === 0) return '';
        const binaryString = window.atob(byteArray);
        const len = binaryString.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        const blob = new Blob([bytes], { type: 'image/jpeg' });
        return URL.createObjectURL(blob);
    }

    function base64ToFile(base64String, filename) {
        let arr, mime, bstr;
        if (base64String.includes(',')) {
            arr = base64String.split(',');
            mime = arr[0].match(/:(.*?);/)[1];
            bstr = atob(arr[1]);
        } else {
            mime = 'image/jpeg';
            bstr = atob(base64String);
        }
    
        let n = bstr.length;
        const u8arr = new Uint8Array(n);
    
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
    
        const blob = new Blob([u8arr], { type: mime });
        return new File([blob], filename, { type: mime });
    }
    

    async function removeUser(userId) {
        try {
            await deleteUserById(userId);
            fetchUsers();
            selectedUser = null;
            M.Modal.getInstance(userDetailsDialog).close();
        } catch (error) {
            console.error('Error deleting user:', error);
        }
    }

    function openEditDialog() {
        const current_image = selectedUser.photo;
        createUserForm('editUserForm', true, selectedUser);
        M.Modal.getInstance(editUserDialog).open();  
        $('#update-user-form').off('submit').on('submit', async function (event) {
            event.preventDefault();
            if (!validateForm()) {
                $('#message').text('Please fill in all required fields correctly.');
                return;
            }
        
            const formData = new FormData();
            formData.append('Username', $('#editUserForm #username').val());
            formData.append('Email', $('#editUserForm #email').val());
            formData.append('BirthDate', $('#editUserForm #birthDate').val());
        
            let photo = $('#editUserForm #photo')[0].files[0];
            if (photo) {
                formData.append('photo', photo);
            } else {
                photo = base64ToFile(current_image,"current_image.jpeg");
                formData.append('photo', photo);
            }
        
            try {
                const response = await createUpdateUser(formData, true);
                $('#message').text(`User updated successfully with ID: ${response.id}`);
                handleReset();
                alert(`User updated successfully with ID: ${response.id}`);
                document.dispatchEvent(new Event('user-updated'));
            } catch (error) {
                $('#message').text(`Error updating user: ${error.message}`);
                alert(`Error updating user: ${error.message}`);
            }
        });   
    }
       

    async function handleUpdate() {
        M.Modal.getInstance(editUserDialog).close();
        fetchUsers();
        if (selectedUser) {
            selectedUser = await getUserById(selectedUser.id); 
        }
    }

    searchQuery.on('input', renderUserList);

    userList.on('click', '.collection-item', function () {
        const userId = $(this).data('id');
        openUserDetails(userId);
    });

    $('#deleteUser').on('click', function () {
        if (selectedUser) {
            removeUser(selectedUser.id);
        }
    });

    $('#updateUser').on('click', function () {
        openEditDialog();
    });

    M.Modal.init(userDetailsDialog[0]);
    M.Modal.init(editUserDialog[0]);

    fetchUsers();
});
