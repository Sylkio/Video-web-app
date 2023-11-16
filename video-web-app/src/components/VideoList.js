import React from 'react';

function VideoList({ videos }) {
    return (
        <div>
            {videos.map(video => (
                <div key={video.id}>
                    <h3>{video.name}</h3>
                    <p>{video.description}</p>
                    {}
                </div>
            ))}
        </div>
    );
}

export default VideoList;
