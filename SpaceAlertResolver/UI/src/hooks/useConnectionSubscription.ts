import { MutableRefObject, useEffect, useState } from 'react';
import { MessageEventData } from '~/models';

export interface useConnectionSubscriptionProps {
	onMessage(messageEventData: MessageEventData): boolean;
	connectionStarted: boolean;
	connectionRef: MutableRefObject<WebSocket | null>;
}

export function useConnectionSubscription({
	onMessage,
	connectionStarted,
	connectionRef,
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
			connectionRef.current?.addEventListener('message', handleMessage);
			setIsSubscribed(true);
		}
		const connectionToClose = connectionRef.current;
		return () => {
			if (connectionStarted) {
				connectionToClose?.removeEventListener('message', handleMessage);
			}
		};
	}, [connectionStarted, connectionRef, onMessage]);

	return {
		isSubscribed,
	};
}
