import { useEffect, useState } from 'react';
import { MessageEventData } from '~/models';

export interface useConnectionSubscriptionProps {
	onMessage(messageEventData: MessageEventData): boolean;
	connectionStarted: boolean;
	connection: WebSocket | null;
}

export function useConnectionSubscription({
	onMessage,
	connectionStarted,
	connection,
}: useConnectionSubscriptionProps) {
	const [isSubscribed, setIsSubscribed] = useState(false);

	useEffect(() => {
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		const handleMessage = (messageEvent: MessageEvent<any>) => {
			const messageEventData = JSON.parse(messageEvent.data) as MessageEventData;
			const success = onMessage(messageEventData);
			if (!success) {
				console.log(`Event unhandled: ${messageEvent.data}`);
			}
			// TODO: Handle error better (and across all subscribers?)
		};
		if (connectionStarted) {
			connection?.addEventListener('message', handleMessage);
			setIsSubscribed(true);
		}
		return () => {
			if (connectionStarted) {
				connection?.removeEventListener('message', handleMessage);
			}
		};
	}, [connectionStarted, connection, onMessage]);

	return {
		isSubscribed,
	};
}
