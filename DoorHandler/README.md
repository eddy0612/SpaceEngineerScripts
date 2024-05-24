        // ---------------------------------------------------------------------------
        // Process all the groups being monitored by this script and set door states
        // correctly.
        // Doors can be tagged [IN] (only open if pressurized) {default if none of the others}
        //                    [OUT] (only open if depressurized)
        //                    [INOUT] (Open anytime)
        // We process all groups before touching the doors as a door might be ok to open
        // in one group but not ok to open in the adjacent group.
        // ---------------------------------------------------------------------------


Door logic

Find all groups tagged [door]
For each group
     find all doors and airvents, ignore anything with [IGNORE]
	// Note: Any door which is not  [out] or [inout] will be assumed to be [in]

     if there are no airvents
	max doors open = 1
	// pretend both pressurized and depressurized
     else
	max doors open = 9999999
	set isfullypressurized correctly
	set isfullydepressurized correctly
     endif

     set opendoors = 0
     for each door
	if isOpen then  /* OPEN DOORS */
		opendoors = opendoors + 1
		Ensure door is enabled - no case where an open door should be disabled
		if door is [in] and not fully pressurized OR
		   door is [out] and not fully depressurized THEN
			add to 'need to close' doors
		endif
	else /* CLOSED DOORS */		
		if opendoors < max doors open AND   /* airlock case */
		   fully depressurized AND door is [out]   OR   /* CLOSED DOORS room in right state */
		   fully pressurized AND door is [in]   OR
		   door is [inout] THEN
			add to 'valid to open' doors

		else   /* CLOSED DOORS room in wrong state */
			add to 'invalid to open' doors
		endif
	endif
    endfor each door
endfor each group

// We now have 3 lists, and one door might be in more than one list
// 1. List of doors we HAVE to close ... set them closing...
// 2. Doors we CANNOT open in the 'invalid to open' list - remove them from the 'valid to open' list, set them to disabled
// 3. Doors we allow people to open - set them to enabled