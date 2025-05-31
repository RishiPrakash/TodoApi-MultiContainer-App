import { useState, useEffect } from 'react';

export const PublicEndpoint = () => {
    const [publicData, setPublicData] = useState('');
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchPublicData = async () => {
            try {
                const response = await fetch('http://localhost:5005/public');
                if (!response.ok) {
                    throw new Error('Failed to fetch public data');
                }
                const data = await response.text();
                setPublicData(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchPublicData();
    }, []);

    if (error) {
        return <div className="alert alert-danger">Error: {error}</div>;
    }

    return (
        <div className="card">
            <div className="card-body">
                <h5 className="card-title">Public API Response</h5>
                <p className="card-text">{publicData}</p>
            </div>
        </div>
    );
};
