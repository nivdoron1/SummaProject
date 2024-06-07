import axios from 'axios';

const API_URL = 'http://localhost:5271/users/createOrUpdateUser'; // Adjust the URL to match your backend's URL

export const createUser = async (formData) => {
    try {
        const response = await axios.post(API_URL, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error creating user:', error);
        throw error;
    }
};
