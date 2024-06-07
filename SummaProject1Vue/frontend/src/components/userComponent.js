import { createUser } from './userService';

export default {
    data() {
        return {
            user: {
                username: '',
                email: '',
                birthDate: '',
                photo: null // Store the file directly
            },
            message: ''
        };
    },
    methods: {
        async handleSubmit() {
            try {
                // Create a FormData object to handle file upload
                const formData = new FormData();
                formData.append('Username', this.user.username);
                formData.append('Email', this.user.email);
                formData.append('BirthDate', this.user.birthDate);
                if (this.user.photo) {
                    formData.append('photo', this.user.photo);
                }
                
                const newUser = await createUser(formData);
                this.message = `User created successfully with ID: ${newUser.id}`;
            } catch (error) {
                this.message = 'Error creating user';
            }
        },
        handleFileChange(event) {
            this.user.photo = event.target.files[0]; // Store the file object directly
        }
    }
};