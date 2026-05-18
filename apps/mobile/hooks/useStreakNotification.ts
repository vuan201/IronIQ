import { useEffect } from 'react';
import * as Notifications from 'expo-notifications';
import { Platform } from 'react-native';

const CHANNEL_ID = 'streak-reminder';
const NOTIFICATION_ID = 'daily-workout-reminder';

async function registerForPushNotifications(): Promise<boolean> {
  const { status: existing } = await Notifications.getPermissionsAsync();
  if (existing === 'granted') return true;

  const { status } = await Notifications.requestPermissionsAsync();
  return status === 'granted';
}

async function scheduleWorkoutReminder(hour: number, minute: number) {
  await Notifications.cancelScheduledNotificationAsync(NOTIFICATION_ID);

  if (Platform.OS === 'android') {
    await Notifications.setNotificationChannelAsync(CHANNEL_ID, {
      name: 'Streak Reminder',
      importance: Notifications.AndroidImportance.DEFAULT,
    });
  }

  await Notifications.scheduleNotificationAsync({
    identifier: NOTIFICATION_ID,
    content: {
      title: '🔥 Đừng để streak bị reset!',
      body: 'Hôm nay bạn chưa tập. Hãy duy trì chuỗi ngày tập của bạn!',
    },
    trigger: {
      type: Notifications.SchedulableTriggerInputTypes.DAILY,
      hour,
      minute,
    },
  });
}

export function useStreakNotification(defaultHour = 20, defaultMinute = 0) {
  useEffect(() => {
    registerForPushNotifications().then((granted) => {
      if (granted) {
        scheduleWorkoutReminder(defaultHour, defaultMinute);
      }
    });
  }, []);
}
