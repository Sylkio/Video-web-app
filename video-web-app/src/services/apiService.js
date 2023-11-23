import axios from 'axios';

const API_BASE_URL = 'http://localhost:5121/api';

export const uploadVideo = async (file) => {
    const formData = new FormData();
    formData.append('file', file);

    try {
        await axios.post(`${API_BASE_URL}/videos/upload`, formData);
        // Handle success
    } catch (error) {
        // Handle error
    }
};

export const getVideos = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/videos`);
        return response.data;
    } catch (error) {
        // Handle error
        return [];
    }
}