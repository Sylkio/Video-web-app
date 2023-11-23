import React, { useState } from 'react';
import { uploadVideo } from '../services/apiService';

const VideoUpload = () => {
    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        if (selectedFile) {
            await uploadVideo(selectedFile);
            // Handle post-upload logic (e.g., refresh video list)


        }
    };

    return (
        <div>
            <input type="file" accept="video/*" onChange={handleFileChange} />
            <button onClick={handleUpload}>Upload Video</button>
        </div>
    );
};

export default VideoUpload;
