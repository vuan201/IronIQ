import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { usersApi } from './api';
import { userKeys } from './query-keys';
import type { UpdateProfileDto } from './types';

export function useMyProfile() {
  return useQuery({
    queryKey: userKeys.me(),
    queryFn: () => usersApi.getMyProfile().then((r) => r.data),
  });
}

export function useUpdateProfile() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: UpdateProfileDto) => usersApi.updateProfile(data).then((r) => r.data),
    onSuccess: (data) => {
      queryClient.setQueryData(userKeys.me(), data);
    },
  });
}
