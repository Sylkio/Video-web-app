import React, { useState } from 'react';

function VideoUpload({ onVideoSelect }) {
    const [videoFile, setVideoFile] = useState(null);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        if (file) {
            const fileType = file.type;
            const fileSize = file.size / 1024 / 1024; // Convert bytes to MB

            if (fileType.match('video/mp4') || fileType.match('video/mov') || 
                fileType.match('video/webm') || fileType.match('video/hevc')) {
                if (fileSize <= 200) {
                    setVideoFile(file);
                } else {
                    alert('File size should be less than 200MB');
                    event.target.value = ''; 
                }
            } else {
                alert('Invalid file type');
                event.target.value = ''; 
            }
        }
    };

    const handleNameChange = (event) => {
        setName(event.target.value);
    };

    const handleDescriptionChange = (event) => {
        setDescription(event.target.value);
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (videoFile) {
            const videoUrl = URL.createObjectURL(videoFile);
            onVideoSelect(videoUrl);

            console.log('Uploading:', videoFile.name);
            console.log('Name:', name);
            console.log('Description:', description);
            //  console logs
        } else {
            alert('Please select a file to upload');
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input type="file" accept="video/*" onChange={handleFileChange} />
                <input type="text" value={name} onChange={handleNameChange} placeholder="Name" />
                <textarea value={description} onChange={handleDescriptionChange} placeholder="Description" />
                <button type="submit">Upload Video</button>
            </form>
        </div>
    );
}

export default VideoUpload;
