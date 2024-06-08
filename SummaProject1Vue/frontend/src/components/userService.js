import axios from 'axios';

const API_URL = 'http://localhost:5271/';

export const createUpdateUser = async (user) => {
    try {
        const response = await axios.post(`${API_URL}users/createOrUpdateUser`, user);
        return response.data;
    } catch (error) {
        console.error('Error creating user:', error);
        throw error;
    }
};

export const getAllUsers = async () => {
    try {
        const response = await axios.get(`${API_URL}users/allUsers`);
        return response.data;
    } catch (error) {
        console.error('Error fetching users:', error);
        throw error;
    }
};

export const getUserById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}users/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching user details:', error);
        throw error;
    }
};

export const deleteUserById = async (id) => {
    try {
        await axios.delete(`${API_URL}users/${id}`);
    } catch (error) {
        console.error('Error deleting user by ID:', error);
        throw error;
    }
};

export const deleteUserByEmail = async (email) => {
    try {
        await axios.delete(`${API_URL}users/email/${email}`);
    } catch (error) {
        console.error('Error deleting user by email:', error);
        throw error;
    }
};
