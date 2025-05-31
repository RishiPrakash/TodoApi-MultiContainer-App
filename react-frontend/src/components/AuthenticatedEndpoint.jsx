import { useState, useEffect } from 'react';
import { useMsal } from '@azure/msal-react';
import { InteractionRequiredAuthError } from '@azure/msal-browser';

export const AuthenticatedEndpoint = () => {
    const { instance } = useMsal();
    const [authenticatedData, setAuthenticatedData] = useState('');
    const [adminData, setAdminData] = useState('');
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const account = instance.getActiveAccount();
                if (!account) {
                    throw new Error('No active account! Please sign in first.');
                }

                const tokenRequest = {
                    scopes: ["https://TestIndiansInIreland.onmicrosoft.com/testIIA-API/testIIA-read"],
                    account: account
                };

                let tokenResponse;
                try {
                    // Try silent token acquisition first
                    tokenResponse = await instance.acquireTokenSilent(tokenRequest);
                } catch (silentError) {
                    if (silentError instanceof InteractionRequiredAuthError) {
                        // If silent token acquisition fails, try interactive
                        console.log("Silent token acquisition failed, trying interactive...");
                        tokenResponse = await instance.acquireTokenRedirect(tokenRequest);
                    } else {
                        throw silentError;
                    }
                }

                console.log("Token acquired successfully, making API call...");
                
                // Regular authenticated endpoint
                const response = await fetch('http://localhost:5005/hello', {
                    headers: {
                        'Authorization': `Bearer ${tokenResponse.accessToken}`
                    }
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    console.error('API Response:', response.status, errorText);
                    throw new Error(`API call failed: ${response.status} ${errorText}`);
                }

                const data = await response.text();
                setAuthenticatedData(data);

                // Try admin endpoint
                try {
                    const adminResponse = await fetch('http://localhost:5005/admin', {
                        headers: {
                            'Authorization': `Bearer ${tokenResponse.accessToken}`
                        }
                    });

                    if (adminResponse.ok) {
                        const adminData = await adminResponse.text();
                        setAdminData(adminData);
                    }
                } catch (adminErr) {
                    console.log('Not an admin or admin access failed:', adminErr);
                }

            } catch (err) {
                console.error('Authentication Error:', err);
                setError(err.message);
                if (err instanceof InteractionRequiredAuthError) {
                    setError("Session expired. Please sign in again.");
                }
            }
        };

        fetchData();
    }, [instance]);

    if (error) {
        return <div className="alert alert-danger">Error: {error}</div>;
    }

    return (
        <div className="card mt-3">
            <div className="card-body">
                <h5 className="card-title">Authenticated API Response</h5>
                <p className="card-text">{authenticatedData}</p>
                {adminData && (
                    <div className="mt-3">
                        <h5 className="card-title">Admin API Response</h5>
                        <p className="card-text">{adminData}</p>
                    </div>
                )}
            </div>
        </div>
    );
};
