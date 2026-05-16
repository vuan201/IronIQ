import { useColorScheme } from 'react-native';
import { StorageKeys, storage } from '../lib/storage';
import { Colors } from '../constants/colors';

export function useTheme() {
  const systemScheme = useColorScheme();
  const saved = storage.getString(StorageKeys.THEME) as 'light' | 'dark' | undefined;
  const scheme = saved ?? systemScheme ?? 'dark';
  return { scheme, colors: Colors[scheme], isDark: scheme === 'dark' };
}
