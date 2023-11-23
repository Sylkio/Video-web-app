import React, { useState, useEffect } from 'react';
import { getVideos } from '../services/apiService';

const VideoList = () => {
    const [videos, setVideos] = useState([]);

    useEffect(() => {
        const fetchVideos = async () => {
            const videoData = await getVideos();
            setVideos(videoData);
        };

        fetchVideos();
    }, []);

    return (
        <div>
            {videos.map(video => (
                <div key={video.id}>
                    <video width="320" height="240" controls>
                        <source src={video.url} type="video/mp4" />
                        Your browser does not support the video tag.
                    </video>
                    <p>{video.name}</p>
                    <p>{video.description}</p>
                </div>
            ))}
        </div>
    );
}

export default VideoList;
